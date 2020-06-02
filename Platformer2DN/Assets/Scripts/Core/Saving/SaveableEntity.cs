using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace platformer.saving
{
    /// <summary>
    /// Each object in a scene that want to save his state must has this component
    /// </summary>
    [ExecuteAlways] // Playmode and edit mode (generating GUID in edit mode)
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = ""; // Will generate if empty

        // This will create one dictionary across all saveable entities
        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>(); 

        // Getter
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier; 
        }

        //For this gameobject, find all Saveables and store them in dictionary
        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(var saveableComponent in GetComponents<ISaveable>())
            {
                //Save each state of this saveable entity to state dictionary
                state[saveableComponent.GetType().ToString()] = saveableComponent.CaptureState();
            }

            return state; // dictionary to save
        }

        // For each saveable component in this saveable entity, pass the state and let it 
        // restore it in his implementation of ISaveable
        public void RestoreState(object state)
        {
            // Cast state into Dictionary<string, object>
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;

            foreach(var saveable in GetComponents<ISaveable>())
            {
                string objectType = saveable.GetType().ToString();
                
                // Check if dictionary contains any saved state for this ISaveable
                if(stateDictionary.ContainsKey(objectType))
                {
                    // Call RestoreState implementation and pass to it his saved state
                    saveable.RestoreState(stateDictionary[objectType]);
                }
            }
        }

        // GENERATING GUID
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(this.gameObject)) return; // If playing, don`t generate GUID
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // If not placed in world, dont generate GUID

            Debug.Log("Editing");

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            // If object doesn`t have GUID or genereated GUID is not unique, create new one
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;

        }

        // Check if generated GUID(candidate) is unique.
        private bool IsUnique(string guidCandidate)
        {
            // If global dictionary doesnt containt this candidate, it`s unique
            if(!globalLookup.ContainsKey(guidCandidate))
            {
                return true;
            }

            // If candidate is himself, it`s unique
            if(globalLookup[guidCandidate] == this)
            {
                return true;
            }

            // If was created but it`s not used anymore, delete previous and it`s unique
            if(globalLookup[guidCandidate] == null)
            {
                globalLookup.Remove(guidCandidate);
                return true;
            }

            // If identifier was changed, remove it and it`s unique
            if(globalLookup[guidCandidate].GetUniqueIdentifier() != guidCandidate)
            {
                globalLookup.Remove(guidCandidate);
                return true;
            }

            // If all previous cases failed, given GUID is not unique
            return false;
        }

#endif
    }

}