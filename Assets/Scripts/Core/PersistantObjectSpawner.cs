using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        public GameObject PersistantObjectPrefab;

        static bool HasSpawned = false;
        private void Awake()
        {
            if (HasSpawned) return;

            SpawnPersistantObject();
            HasSpawned = true;

        }
        private void SpawnPersistantObject()
        {
            GameObject persistantObject = Instantiate(PersistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}
