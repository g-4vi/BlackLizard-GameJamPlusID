using System.Collections;
using UnityEngine;

public class GoblinBehaviour : ObstacleProperties
{

    [Header("References")]
    [SerializeField] GameObject _arrowPrefab;
    [SerializeField] float _shootInterval = 2f;
    public Animator anim;
    public SpriteRenderer _spriteRenderer;
   
    public int AttackHash {  get; set; }

    private void Awake()
    {
        AttackHash = Animator.StringToHash("attack");
    }
    void SpawnProjectile()
    {
        if (_specialSound != SfxID.None) AudioManager.Instance.PlaySFX(_specialSound);
       
        GameObject projectile = Instantiate(_arrowPrefab, transform.position, Quaternion.identity);
        if (projectile.TryGetComponent<ObstacleProperties>(out ObstacleProperties obs))
        {
            obs.SetDirection(_direction);
        }
    }

    IEnumerator ShootProjectile()
    {
        
        yield return new WaitForSeconds(_shootInterval);
        while (true)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(_shootInterval);
        }
    } 

    public void ActivateShooting()
    {
        StartCoroutine(ShootProjectile());
    }
}
