using System.Collections.Generic;
using UnityEngine;

namespace GameJamPlus.ManaModules {
    public class ManaSpawner : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject prefabToSpawn;
        [SerializeField] List<Transform> spawnPoints = new List<Transform>();

        [Header("Settings")]
        [SerializeField] float spawnInterval = 2f;

        GameObject currentSpawnedItem;

        void Awake() {
            if (spawnPoints.Count == 0) { // If no spawn points assigned, use children transforms
                spawnPoints.AddRange(GetComponentsInChildren<Transform>());
                spawnPoints.Remove(transform); // Remove self from spawn points

                if (spawnPoints.Count == 0)
                    Debug.LogWarning($"[{name}] No spawn points assigned for ManaSpawner.");
            }
        }

        void Start() {
            SpawnItem();
        }

        void SpawnItem() {
            if (prefabToSpawn == null || spawnPoints.Count == 0) return;

            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            currentSpawnedItem = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
            var manaItem = currentSpawnedItem.GetComponent<ManaItem>();

            if (manaItem != null) {
                manaItem.OnCollected += StartSpawnTimer;
            }

            // TODO: Spawn sound effect ?

            Debug.Log($"[{name}] Spawned ManaItem at {spawnPoint.position}");
        }

        void StartSpawnTimer() {
            Invoke(nameof(SpawnItem), spawnInterval);
        }

    }
}