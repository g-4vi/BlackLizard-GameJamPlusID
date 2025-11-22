using Mono.Cecil;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    List<GameObject> _existingObstacles;


    [Header("Properties")]
    [SerializeField] GameObject _indicatorObject;
    [SerializeField] float _spawnInterval = 6f;

    [Header("References")]
    [Header("Spiders")]
    [SerializeField] GameObject _spiderPrefab;
    [SerializeField] List<GameObject> _spiderSpawnArea;
    [SerializeField] List<GameObject> _spiderStayPoints;

    [Header("Bat")]
    [SerializeField] GameObject _batPrefab;
    [SerializeField] List<GameObject> _batSpawnArea;
    [SerializeField] List<Vector3> _batPathPoints;

    [Header("Boulder")]
    [SerializeField] GameObject _boulderPrefab;
    [SerializeField] List<GameObject> _boulderSpawnArea;

    [Header("Spikes")]
    [SerializeField] GameObject _spikePrefab;
    [SerializeField] List<GameObject> _spikeSpawnArea;

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
            SpawnRandomObstacle();
            yield return new WaitForSeconds(_spawnInterval+3.2f);
        }
    }
    
    

    void SpawnRandomObstacle()
    {
        Debug.Log("Spawning random obstacle...");
        int randNum = Random.Range(1, 5);

        switch (randNum)
        {
            case (1):
                Debug.Log("Spawning bat!");
                StartCoroutine(SpawnBat(_batSpawnArea[Random.Range(0, _batSpawnArea.Count)].transform.position));
                break;
            case (2):
                Debug.Log("Spawning spider!");
                StartCoroutine(SpawnSpider(_spiderStayPoints[Random.Range(0, _spiderStayPoints.Count)].transform.position));
                break;
            case (3):
                Debug.Log("Spawning boulder!");
                StartCoroutine(SpawnBoulder(_boulderSpawnArea[Random.Range(0, _boulderSpawnArea.Count)].transform.position));
                break;
            case (4):
                Debug.Log("Spawning spike trap!");
                StartCoroutine(SpawnSpike(_spikeSpawnArea[Random.Range(0, _spikeSpawnArea.Count)].transform.position));
                break;
        }
    }

    /*NOTE: Spider berbeda, karena targetPoint nya bukan tempat spawn awalnya. Tapi spawnnya di titik dipaling atas diatas targetPoint, 
     * dan targetPointnya adalah tempat akhir dari spidernya */
    IEnumerator SpawnSpider(Vector3 targetPoint) 
    {
        GameObject indicator = SpawnIndicator(_indicatorObject, targetPoint);
        indicator.GetComponent<Animator>().SetTrigger("BlinkEffect");
        yield return new WaitForSeconds(3.2f);
        Destroy(indicator);
        //
        GameObject obj;
        if (targetPoint.x < 0)
        {
            obj = Instantiate(_spiderPrefab, _spiderSpawnArea[0].transform.position, Quaternion.identity);    
        }
        else
        {
            obj = Instantiate(_spiderPrefab, _spiderSpawnArea[1].transform.position, Quaternion.identity);
        }
        while (Vector3.Distance(obj.transform.position, targetPoint) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPoint, Time.deltaTime * 3f);
            yield return null;
        }


        SpiderBehaviour spider = obj.GetComponent<SpiderBehaviour>();
        if (targetPoint.x < 0)
            {
                spider.SetDirection(Vector3.right);
            }
            else
            {
                spider.SetDirection(Vector3.left);
            }

        spider.ActivateShooting();
        }


   

    IEnumerator SpawnBat(Vector3 targetPoint)
    {
        GameObject indicator;
        if (targetPoint.x < 0)
        {
            indicator = SpawnIndicator(_indicatorObject, new Vector3(targetPoint.x+1.5f,targetPoint.y,targetPoint.z));
        }
        else
        {
            indicator = SpawnIndicator(_indicatorObject, new Vector3(targetPoint.x - 1.5f, targetPoint.y, targetPoint.z));
        }
        indicator.GetComponent<Animator>().SetTrigger("BlinkEffect");
        yield return new WaitForSeconds(3.2f);
        Destroy(indicator);
        GameObject obj;
        obj = Instantiate(_batPrefab, targetPoint, Quaternion.identity);

        
        if (obj.TryGetComponent<BatBehaviour>(out BatBehaviour batBehaviour))
        {
            Shuffle<Vector3>(_batPathPoints);
            _batPathPoints.Insert(0,targetPoint);
            batBehaviour.EditPathPoint(_batPathPoints);
            _batPathPoints.RemoveAt(0);
        }
        
    }

    IEnumerator SpawnBoulder(Vector3 targetPoint)
    {
        GameObject indicator;
        if (targetPoint.x < 0)
        {
            indicator = SpawnIndicator(_indicatorObject, new Vector3(targetPoint.x + 1.5f, targetPoint.y, targetPoint.z));
        }
        else
        {
            indicator = SpawnIndicator(_indicatorObject, new Vector3(targetPoint.x - 1.5f, targetPoint.y, targetPoint.z));
        }
        indicator.GetComponent<Animator>().SetTrigger("BlinkEffect");
        yield return new WaitForSeconds(3.2f);
        Destroy(indicator);

        GameObject obj;
        obj = Instantiate(_boulderPrefab, targetPoint, Quaternion.identity);

        BoulderBehaviour boulder = obj.GetComponent<BoulderBehaviour>();
            if (targetPoint.x < 0)
            {
                Debug.Log("Going right + " + targetPoint);
            boulder.SetDirection(Vector3.right);
            }
            else
            {
                Debug.Log("Going left + "+targetPoint);
            boulder.SetDirection(Vector3.left);
            }
        

    }

    IEnumerator SpawnSpike(Vector3 targetPoint) {

        GameObject indicator = SpawnIndicator(_indicatorObject , targetPoint);
        indicator.GetComponent<Animator>().SetTrigger("BlinkEffect");
        yield return new WaitForSeconds(3.2f);
        Destroy(indicator);

        GameObject obj;
        obj = Instantiate(_spikePrefab, targetPoint, Quaternion.identity);
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
