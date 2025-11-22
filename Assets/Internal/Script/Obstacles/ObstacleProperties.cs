using System;
using UnityEngine;


public abstract class ObstacleProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected float _objectSpeed = 5f;
    [SerializeField] protected float _objectHealth = 1f;
    [SerializeField] protected float _objectDamage = 1f;
    protected Vector3 direction;

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

    void DealDamageToPlayer(float damage)
    {
        // Access Player Health Component
        // Subtract Damage from Player Health
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(_objectDamage);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
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
    }



    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
}
