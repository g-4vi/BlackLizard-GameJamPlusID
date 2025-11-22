using System.Collections;
using UnityEngine;

public class SpiderBehaviour : ObstacleProperties
{

    [Header("References")]
    [SerializeField] GameObject _spiderWebPrefab;
    [SerializeField] float _webShootInterval = 2f;

    void SpawnProjectile()
    {
        GameObject projectile = Instantiate(_spiderWebPrefab, transform.position, Quaternion.identity);
        if (projectile.TryGetComponent<ObstacleProperties>(out ObstacleProperties obs))
        {
            obs.SetDirection(direction);
        }
    }

    IEnumerator ShootWeb()
    {
        yield return new WaitForSeconds(_webShootInterval);
        while (true)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(_webShootInterval);
        }
    } 

    public void ActivateShooting()
    {
        StartCoroutine(ShootWeb());
    }
}
