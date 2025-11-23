using System.Collections;
using UnityEngine;

public class SpikeTrapBehaviour : ObstacleProperties
{

    [SerializeField] private float _persistTime = 2f;
    Collider2D _objectCollider;
    Animator _objectAnimator;

    SpriteRenderer _sprite;

    void Awake()
    {
        _objectCollider = GetComponent<Collider2D>();
        _objectAnimator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = true;
        _objectCollider.enabled = false;
        StartCoroutine(Appear());
    }


    IEnumerator Appear()
    {
        //Tremor Effect
        _sprite.enabled = true; // temporary
        _objectCollider.enabled = true;
        //Play animation

        yield return new WaitForSeconds(_persistTime);

        _sprite.enabled = false; // temporary
        _objectCollider.enabled = false;
        Destroy(this.gameObject);
    }

    // Ini engga kepake soalnya udah di handle di ObstacleProperties

    //void DealDamageToPlayer(int damage)
    //{
    //    // Access Player Health Component
    //    // Subtract Damage from Player Health

    //    Player player = PlayerManager.Instance.playerInstance;
    //    if (!player.IsInvisible)
    //    {
    //        Debug.Log("Ouch! GOt hit");
    //        PlayerManager.Instance.TakeDamage(damage);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        DealDamageToPlayer(_objectDamage);
    //        Physics2D.IgnoreCollision(collision.GetComponent<CapsuleCollider2D>(), GetComponent<Collider2D>());
    //    }
    //}


}
