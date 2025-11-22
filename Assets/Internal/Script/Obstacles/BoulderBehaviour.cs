using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderBehaviour : ObstacleProperties
{
    //[SerializeField] float _objectSpeed = 5f;
    //[SerializeField] float _objectHealth = 1f;
    //[SerializeField] float _objectDamage = 1f;

    [Header("Refereneces")]
    [SerializeField] Animator _animator;

    void Update()
    {
        transform.position = new Vector3(transform.position.x + _objectSpeed * Time.deltaTime, transform.position.y , transform.position.z);
    }

    IEnumerator RollToRight()
    {
        // Play Roll Animation
        // Move Object to the Right

        yield return null;
    }

    public void RollToLeft()
    {

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
        }
    }
}
