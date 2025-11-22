using System;
using UnityEngine;


public abstract class ObstacleProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected float _objectSpeed = 5f;
    [SerializeField] protected float _objectHealth = 1f;
    [SerializeField] protected int _objectDamage = 1;
    protected Vector3 direction;

    [Header("Knockback")]
    [SerializeField] protected float knockbackForce = 5f;
    [SerializeField] protected float knockbackDuration = 0.2f;


    protected void Update()
    {
        if (this._objectHealth <= 0 || OutOfScreen())
        {
            Destroy(this.gameObject);
        }
    }

    bool OutOfScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.3 || screenPoint.x > 1.3 || screenPoint.y < -0.3 || screenPoint.y > 1.3;
    }

    void DealDamageToPlayer(int damage)
    {
        // Access Player Health Component
        // Subtract Damage from Player Health
        Player player = PlayerManager.Instance.player.GetComponent<Player>();
        if (!player.IsInvisible)
        {
            Debug.Log("Ouch! GOt hit");
            player.playerProperties.UpdateHealth(-damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))//for boulders
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Debug.Log("Fireball hit!");
            Destroy(collision.gameObject);
            _objectHealth--;
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(_objectDamage);
            Collider2D[] playerColliders = collision.GetComponentsInParent<Collider2D>();
            foreach (var col in playerColliders)
                Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());

            Transform player = collision.gameObject.transform;
            Vector2 direction = (player.position - transform.position).normalized;//direction of obstacle, + is from left
            direction = new Vector2(Mathf.Sign(direction.x), 0);//only horizontal knockback
            player.GetComponent<PlayerMovement>().OnDamaged(direction, knockbackForce, knockbackDuration);

            if (gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                Destroy(gameObject);
        }
    }

   

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
}
