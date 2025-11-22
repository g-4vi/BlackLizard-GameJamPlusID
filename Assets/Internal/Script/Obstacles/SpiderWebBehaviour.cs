using System.Collections;
using UnityEngine;

public class SpiderWebBehaviour : ObstacleProperties
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FlyToDirection(direction));
    }
    IEnumerator FlyToDirection(Vector3 direction)
    {
        // Play Fly Animation
        while (true)
        {
            transform.position = new Vector3(transform.position.x + direction.x * _objectSpeed * Time.deltaTime,
                                             transform.position.y + direction.y * _objectSpeed * Time.deltaTime,
                                             transform.position.z + direction.z * _objectSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
