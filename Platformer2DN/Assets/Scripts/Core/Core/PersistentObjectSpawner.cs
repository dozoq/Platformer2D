using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.core
{
    /// <summary>
    /// Each time that you start playing, this class will spawn persistent objects that won`t be destroyed
    /// between scenes
    /// </summary>
    public class PersistentObjectSpawner : MonoBehaviour
    {
        // Prefab that contains all persistent gameobjects that will be spawned on Load.
        [SerializeField] private GameObject persistentObjectToSpawn = null;

        private static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            if (persistentObjectToSpawn == null) 
            {
                Debug.LogError("No persistent object to spawn!");
                return;
            }

            SpawnPersistentObject();

            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectToSpawn);
            DontDestroyOnLoad(persistentObject); // Add this gameobject to persistent objects between scenes
        }
    }

}