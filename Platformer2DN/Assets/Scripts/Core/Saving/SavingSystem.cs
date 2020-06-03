using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace platformer.saving
{
    /// <summary>
    ///  Script responsible for reading save and loading from/to file
    /// </summary>
    public class SavingSystem : MonoBehaviour
    {

        //Called from SavingWrapper when we want to save
        public void Save(string saveFile)
        {
            // Load previous save, add what`s changed and save to .sav file
            Dictionary<string, object> state = LoadFromFile(saveFile); // Previously saved state
            CaptureState(state); // Add to state dictionary
            SaveToFile(saveFile, state); // Save state dictionary
        }

        // Load last save
        public void Load(string saveFile)
        {
            RestoreState(LoadFromFile(saveFile));
        }

        // Delete save file
        public void Delete(string saveFile)
        {

            File.Delete(GetPathFromSaveFile(saveFile));
        }

        // Load saved dictionary of all states
        private Dictionary<string, object> LoadFromFile(string saveFile)
        {
            // Get path to given file
            string savePath = GetPathFromSaveFile(saveFile);

            // If save not exists, return empty dictionary with no saved states
            if(!File.Exists(savePath))
            {
                return new Dictionary<string, object>();
            }

            // Reading data from file
            using(FileStream stream = File.Open(savePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter(); // Deserializer
                // Deserialize whats saved in .sav file into Dictionary<string, objects>;
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        // Save dictionary of all states into saveFile, captureState - dictionary<string, object>
        private void SaveToFile(string saveFile, object captureState)
        {
            string savePath = GetPathFromSaveFile(saveFile);

            // This will remove eveyrthing from the file and save to it again if not exists
            using (FileStream stream = File.Open(savePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, captureState); // Serialize Dictionary into this stream file
            }
        }

        //Capture state of all SaveableEntities in the scene
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                // This will call SaveableEntity CaptureState, which looks for all ISaveable components
                // in his GameObject, and save all their state into dictionary, which will be stored
                // in state Dictionary and saved in .sav file
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            // Add to Dictionary build index of the active scene
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }


        // Restore state for all saveable entity in the scene
        private void RestoreState(Dictionary<string, object> deserializeState)
        {
            //if Dictionary is not empty(nothing saved)
            if(deserializeState != null)
            {
                foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
                {
                    string guid = saveable.GetUniqueIdentifier();

                    // If saved file had a state of this saveable, restore it state
                    if(deserializeState.ContainsKey(guid))
                    {
                        saveable.RestoreState(deserializeState[guid]);
                    }
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
