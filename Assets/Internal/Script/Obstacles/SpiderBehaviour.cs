using System.Collections;
using UnityEngine;

public class SpiderBehaviour : ObstacleProperties
{

    [Header("References")]
    [SerializeField] GameObject _spiderWebPrefab;
    [SerializeField] float _webShootInterval = 2f;

    void SpawnProjectile()
    {
        if (_specialSound != SfxID.None) AudioManager.Instance.PlaySFX(_specialSound);
       
        GameObject projectile = Instantiate(_spiderWebPrefab, transform.position, Quaternion.identity);
        if (projectile.TryGetComponent<ObstacleProperties>(out ObstacleProperties obs))
        {
            obs.SetDirection(_direction);
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
