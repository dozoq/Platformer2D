using platformer.saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.scenemanagment
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFileName = "save";

        [SerializeField] private float loadingFaderTime = 5f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        // Wil load last saved scene and restore state of all saveable entities
        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFileName);
            LoadingFader loadingFader = FindObjectOfType<LoadingFader>();
            loadingFader.FadeOutInstantly();
            yield return loadingFader.FadeIn(loadingFaderTime);

        }

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

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFileName);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFileName);
            //StartCoroutine(LoadLastScene());
        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFileName);
        }
    }

}