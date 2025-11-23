using Mono.Cecil;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//spaghetti code ges maap, nanti kupisah jadi spawner masing masing
public class ObstacleManager : MonoBehaviour
{
    List<GameObject> _existingObstacles;


    [Header("Properties")]
    [SerializeField] GameObject _indicatorObject;
    [SerializeField] float _spawnInterval = 6f;
    [SerializeField] SfxID _indicatorWarning;

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
    ObstacleProperties lastObsSpawned;

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
        yield return new WaitForSeconds(_spawnInterval);
        while (true)
        {
            SpawnRandomObstacle();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    
    

    void SpawnRandomObstacle()
    {
        Debug.Log("Spawning random obstacle...");
        

        GameObject obj;
        for (int i = 0;i<10;i++) // Try 10 times to find a free slot, else
        {
            int randNum = Random.Range(1, 5);
            lastObsSpawned = null;
            switch (randNum)
            {
                case (1):
                    Debug.Log("Spawning bat!");
                    obj = _batSpawnArea[Random.Range(0, _batSpawnArea.Count)];
                    if (obj.TryGetComponent<ObstacleSlot>(out ObstacleSlot obsA) && obsA.OccupiedStatus) continue;

                    // Bat gaperlu ubah slot obstacle
                    StartCoroutine(SpawnBat(obj.transform.position));
                    ;
                    break;
                case (2):
                    Debug.Log("Spawning spider!");
                    obj = _spiderStayPoints[Random.Range(0, _spiderStayPoints.Count)];
                    if (obj.TryGetComponent<ObstacleSlot>(out ObstacleSlot obsB) && obsB.OccupiedStatus) continue;

                    obsB.ChangeOccupyStatus(true);
                    // TODO : Add a logic to reference spawned spider, so we can call the assignSlot function
                    StartCoroutine(SpawnSpider(obj.transform.position, obsB));
                    
                    break;
                case (3):
                    Debug.Log("Spawning boulder!");
                    obj = _boulderSpawnArea[Random.Range(0, _boulderSpawnArea.Count)];
                    if (obj.TryGetComponent<ObstacleSlot>(out ObstacleSlot obsC) && obsC.OccupiedStatus) continue;

                    // Boulder gaperlu ubah slot obstacle
                    StartCoroutine(SpawnBoulder(obj.transform.position));
                    break;
                case (4):
                    Debug.Log("Spawning spike trap!");
                    obj = _spikeSpawnArea[Random.Range(0, _spikeSpawnArea.Count)];
                    if (obj.TryGetComponent<ObstacleSlot>(out ObstacleSlot obsD) && obsD.OccupiedStatus) continue;

                    obsD.ChangeOccupyStatus(true);
                    // TODO : Add a logic to reference spawned spike, so we can call the assignSlot function
                    StartCoroutine(SpawnSpike(obj.transform.position,obsD));
                    
                    break;
            }
            break;
        }
        
    }

    /*NOTE: Spider berbeda, karena targetPoint nya bukan tempat spawn awalnya. Tapi spawnnya di titik dipaling atas diatas targetPoint, 
     dan targetPointnya adalah tempat akhir dari spidernya */
    IEnumerator SpawnSpider(Vector3 targetPoint, ObstacleSlot obsSlot) 
    {
        GameObject indicator = SpawnIndicator(_indicatorObject, targetPoint);
        Animator anim = indicator.GetComponent<Animator>();
        anim.SetTrigger("BlinkEffect");
        yield return new WaitUntil(() =>
       anim.GetCurrentAnimatorStateInfo(0).IsName("IndicatorBlink")
   );
        float clipLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clipLength);
        Destroy(indicator);
        //
        GameObject obj;
        Vector3 spawnPos = targetPoint.x < 0 ? _spiderSpawnArea[0].transform.position
                                             : _spiderSpawnArea[1].transform.position;

        obj = Instantiate(_spiderPrefab, spawnPos, Quaternion.identity);
        lastObsSpawned = obj.GetComponent<SpiderBehaviour>();
        while (Vector3.Distance(obj.transform.position, targetPoint) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPoint, Time.deltaTime * 3f);
            yield return null;
        }

        SpiderBehaviour spider = obj.GetComponent<SpiderBehaviour>();
        spider.SetDirection(
            targetPoint.x < 0 ? Vector3.right : Vector3.left
        );
        spider.AssignSlot(obsSlot);
        spider.ActivateShooting();
        }

    //FOR DEBUGGING PURPOSES
    [ContextMenu("Spawn Spider")]
    void SpawnRandomSpider()
    {
        GameObject obj;
        for (int i = 0; i < 10; i++)
        {
            obj = _spiderStayPoints[Random.Range(0, _spiderStayPoints.Count)];
            if (obj.TryGetComponent<ObstacleSlot>(out ObstacleSlot obsSlot) && obsSlot.OccupiedStatus) continue;
            obsSlot.ChangeOccupyStatus(true);
            Debug.Log("Spawning spider manually!");
            StartCoroutine(SpawnSpider(obj.transform.position, obsSlot));
            
            break;
        }
        
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
        Animator anim = indicator.GetComponent<Animator>();
        anim.SetTrigger("BlinkEffect");
        yield return new WaitUntil(() =>
       anim.GetCurrentAnimatorStateInfo(0).IsName("IndicatorBlink")
   );
        float clipLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clipLength);
        Destroy(indicator);
        GameObject obj;
        obj = Instantiate(_batPrefab, targetPoint, Quaternion.identity);
        lastObsSpawned = obj.GetComponent<BatBehaviour>();

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
        Animator anim = indicator.GetComponent<Animator>();
        anim.SetTrigger("BlinkEffect");
        yield return new WaitUntil(() =>
       anim.GetCurrentAnimatorStateInfo(0).IsName("IndicatorBlink")
   );
        float clipLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clipLength);
        Destroy(indicator);

        GameObject obj;
        obj = Instantiate(_boulderPrefab, targetPoint, Quaternion.identity);
        lastObsSpawned = obj.GetComponent<BoulderBehaviour>();

        BoulderBehaviour boulder = obj.GetComponent<BoulderBehaviour>();
        boulder.SetDirection(
            targetPoint.x < 0 ? Vector3.right : Vector3.left
        );
    }

    IEnumerator SpawnSpike(Vector3 targetPoint, ObstacleSlot obsSlot) {

        GameObject indicator = SpawnIndicator(_indicatorObject , targetPoint);
        Animator anim = indicator.GetComponent<Animator>();
        anim.SetTrigger("BlinkEffect");
        yield return new WaitUntil(() =>
       anim.GetCurrentAnimatorStateInfo(0).IsName("IndicatorBlink")
   );
        float clipLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clipLength);
        Destroy(indicator);

        GameObject obj;
        obj = Instantiate(_spikePrefab, targetPoint, Quaternion.identity);
        lastObsSpawned = obj.GetComponent<SpikeTrapBehaviour>();
        lastObsSpawned.AssignSlot(obsSlot);
    }

   

    GameObject SpawnIndicator(GameObject indicatorPrefab, Vector3 spawnPosition)
    {
        if (_indicatorWarning!= SfxID.None) AudioManager.Instance.PlaySFX(_indicatorWarning);
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
