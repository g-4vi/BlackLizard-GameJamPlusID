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
        StartCoroutine(RollToRight());
    }

    IEnumerator RollToRight()
    {
        // Play Roll Animation
        
        while (true)
        {

            transform.position = new Vector3(transform.position.x + _objectSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            transform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
            yield return null;
        }
        
    }

    public void RollToLeft()
    {

    }
}
