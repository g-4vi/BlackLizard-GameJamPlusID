using System;
using UnityEngine;


public abstract class ObstacleProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected float _objectSpeed = 5f;
    [SerializeField] protected float _objectHealth = 1f;
    [SerializeField] protected float _objectDamage = 1f;
    protected Vector3 direction;
    protected ObstacleSlot obstacleSlot;

    protected void Update()
    {
        if (this._objectHealth <= 0 || OutOfScreen())
        {
            DestroyObstacle();
        }
    }

    bool OutOfScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.3 || screenPoint.x > 1.3 || screenPoint.y < -0.3 || screenPoint.y > 1.3;
    }

    void DestroyObstacle()
    {
        // TODO: Play destroy anim
        if (obstacleSlot != null) obstacleSlot.ChangeOccupyStatus(false);
        Destroy(this.gameObject);
    }

    void DealDamageToPlayer(float damage)
    {
        // TODO: Access Player Health Component
        // TODO: Subtract Damage from Player Health
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Debug.Log("Fireball hit!");
            Destroy(collision.gameObject);
            _objectHealth--;
        } else if (collision.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(_objectDamage);
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
    public void AssignSlot(ObstacleSlot slotObj)
    {
        obstacleSlot = slotObj;
    }
}
