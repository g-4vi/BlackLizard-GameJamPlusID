using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoarBehaviour : ObstacleProperties
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Move(_direction));
    }


    IEnumerator Move(Vector3 direction)
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x + direction.x * _objectSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            
            yield return null;
        }
    }


    
}
