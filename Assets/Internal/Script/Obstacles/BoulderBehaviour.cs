using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderBehaviour : ObstacleProperties
{
    float _rotationSpeed = 200f;

    

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Roll(_direction));
    }


    IEnumerator Roll(Vector3 direction)
    {
        
        while (true)
        {
            transform.position = new Vector3(transform.position.x + direction.x * _objectSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            transform.Rotate(0, 0, -(_rotationSpeed*direction.x) * Time.deltaTime);
            yield return null;
        }
    }


    
}
