using System.Collections;
using UnityEngine;

public class SpiderBehaviour : ObstacleProperties
{

    [Header("References")]
    [SerializeField] GameObject _spiderWebPrefab;
    [SerializeField] float _webShootInterval = 2f;


    private void Start()
    {
        StartCoroutine(ShootWeb());
    }

    // Update is called once per frame


    
    void SpawnWeb()
    {
        Instantiate(_spiderWebPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator ShootWeb()
    {
        while (true)
        {
            SpawnWeb();
            yield return new WaitForSeconds(_webShootInterval);
        }
    } 
}
