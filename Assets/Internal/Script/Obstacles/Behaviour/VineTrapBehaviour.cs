using System.Collections;
using UnityEngine;

public class VineTrapBehaviour : ObstacleProperties
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

        //Appear();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player hit obstacle!");
        Appear();
        DealDamageToPlayer(_objectDamage);
        Collider2D[] playerColliders = collision.GetComponentsInParent<Collider2D>();
        foreach (var col in playerColliders)
            Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());

        Transform player = collision.gameObject.transform;
        Vector2 direction = (player.position - transform.position).normalized;//direction of obstacle, + is from left
        direction = new Vector2(Mathf.Sign(direction.x), 0);//only horizontal knockback
        player.GetComponent<PlayerMovement>().OnDamaged(direction, knockbackForce, knockbackDuration);

        if (gameObject.name.ToLower().Contains("boulder")) return;//boulder objects dont get destroyed when hit player

        StartCoroutine(DelayedDestroy());
    }

    void Appear()
    {
        //Tremor Effect
        _sprite.enabled = true; // temporary
        _objectAnimator.SetTrigger("Appear");
        
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_persistTime);
        DestroyObstacle();
    }

    


}
