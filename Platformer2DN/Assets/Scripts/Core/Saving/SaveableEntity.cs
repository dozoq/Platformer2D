using System.Collections;
using System.Collections.Generic;
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
                state[saveableComponent.GetType().ToString()] = saveableComponent.CaptureState();
            }

            return state;
        }


    }

}