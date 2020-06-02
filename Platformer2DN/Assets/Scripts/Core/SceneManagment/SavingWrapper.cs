using platformer.saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.scenemanagment
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFileName = "save";

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                print("save");
                Save();
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Load();
            }

            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                Delete();
            }
        }

        private void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFileName);
        }

        private void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFileName);
        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFileName);
        }
    }

}