using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    List<GameObject> _existingObstacles;
    [SerializeField] List<GameObject> _obstaclePrefabs;
    [SerializeField] List<GameObject> _indicatorPrefabs;

    [Header("Properties")]
    [SerializeField] float _spawnInterval = 6f;

    [Header("References")]
    [SerializeField] List<GameObject> _spikeSpawnArea;
    [SerializeField] List<GameObject> _batSpawnArea;
    [SerializeField] List<GameObject> _spiderSpawnArea;
    [SerializeField] List<GameObject> _boulderSpawnArea;

    [SerializeField] List<GameObject> _batPathPoints;
    [SerializeField] List<GameObject> _spawnAreas;

    Animator _animator;

    private float _elapsedTime = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(AutoSpawnObstacles());
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= 60 && _spawnInterval > 2f)
        {
            _spawnInterval -= 0.5f;
            _elapsedTime = 0f;       
        }
    }

    IEnumerator AutoSpawnObstacles()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, _obstaclePrefabs.Count);
            int randomAreaIndex = Random.Range(0, _spawnAreas.Count);

            StartCoroutine(SpawnObstacle(_obstaclePrefabs[randomIndex], _spawnAreas[randomAreaIndex].transform.position));
            yield return new WaitForSeconds(_spawnInterval+3.2f);
        }
    }
    
    IEnumerator SpawnObstacle(GameObject obstaclePrefab, Vector3 spawnPosition)
    {
        GameObject indicator = SpawnIndicator(_indicatorPrefabs[0], spawnPosition);
        indicator.GetComponent<Animator>().SetTrigger("BlinkEffect");

        yield return new WaitForSeconds(3.2f);
        Destroy(indicator);
        
        GameObject obj = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        if (obj.TryGetComponent<BatBehaviour>(out BatBehaviour batBehaviour))
        {
            Shuffle<GameObject>(_batPathPoints);
            batBehaviour.EditPathPoint(_batPathPoints);
        }
        else if (obj.TryGetComponent<ObstacleProperties>(out ObstacleProperties obs))
        {
            if (spawnPosition.x < 0)
            {
                obs.SetDirection(Vector3.right);
            } else
            {
                obs.SetDirection(Vector3.left);
            }   
        }
        
    }

   

    GameObject SpawnIndicator(GameObject indicatorPrefab, Vector3 spawnPosition)
    {
          return Instantiate(indicatorPrefab, spawnPosition, Quaternion.identity);
    }

    // Algoritma Fisher-Yates Shuffle 
    public static void Shuffle<T>(List<T> list)
    {
       
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1); 
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}
