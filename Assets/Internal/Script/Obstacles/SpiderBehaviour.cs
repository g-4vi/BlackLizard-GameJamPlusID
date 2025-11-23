using System.Collections;
using UnityEngine;

public class SpiderBehaviour : ObstacleProperties
{

    [Header("References")]
    [SerializeField] GameObject _spiderWebPrefab;
    [SerializeField] float _webShootInterval = 2f;
    public Animator anim;
    public SpriteRenderer _spiderSpriteRenderer;
   
    public int AttackHash {  get; set; }

    private void Awake()
    {
        AttackHash = Animator.StringToHash("attack");
    }
    void SpawnProjectile()
    {
        AudioManager.Instance.PlaySFX(_specialSound);
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
