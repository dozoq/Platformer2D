using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer.core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
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
            DontDestroyOnLoad(persistentObject);
        }
    }

}