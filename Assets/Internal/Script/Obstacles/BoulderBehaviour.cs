using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderBehaviour : ObstacleProperties
{
    float _rotationSpeed = 200f;


    [Header("Refereneces")]
    [SerializeField] Animator _animator;

    private void Start()
    {

        StartCoroutine(Roll(_direction));
    }


    IEnumerator Roll(Vector3 direction)
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x + direction.x * _objectSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            transform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


    
}
