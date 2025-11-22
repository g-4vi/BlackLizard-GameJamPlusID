using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatBehaviour : ObstacleProperties
{
    [Header("References")]
    [SerializeField] Animator _animator;
    [SerializeField] List<GameObject> _pathPoints;

    private void Start()
    {
        int pattern = Random.Range(0, 10);
        StartCoroutine(FlyFixedPath(_pathPoints));

        //if (pattern > 5)
        //{
        //    StartCoroutine(FlyStraight(Vector3.left));
        //}
        //else
        //{
        //    StartCoroutine(FlyRandomZigZag(Vector3.left));
        //}
    }

    public void EditPathPoint(List<GameObject> newPathPoints)
    {
        _pathPoints = newPathPoints;
    }

    IEnumerator FlyStraight(Vector3 direction)
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

    IEnumerator FlyRandomZigZag(Vector3 startingDirection)
    {
        float elapsedTime = 0f;
        while (true)
        {
            float zigzagThreshold = 2f; // Adjust this value to change the zig-zag frequency
            transform.position = new Vector3(transform.position.x + startingDirection.x * _objectSpeed * Time.deltaTime,
                                             transform.position.y + startingDirection.y * _objectSpeed * Time.deltaTime,
                                             transform.position.z + startingDirection.z * _objectSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= zigzagThreshold)
            {
                elapsedTime = 0f; // Reset elapsed time to create continuous zig-zag
                startingDirection = new Vector3(-startingDirection.x, Random.Range(-1, 1), transform.position.z);
                yield return new WaitForSeconds(1.5f);
            } else
            {
                yield return null;
            }
                
        }
    }

    IEnumerator FlyFixedPath(List<GameObject> pathPoints)
    {
        int idx = 0;

        Vector3 heading = pathPoints[idx].transform.position - transform.position;
        Vector3 direction = heading / heading.magnitude;
        // Play Fly Animation
        while (true)
        {
            transform.Translate(direction*_objectSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pathPoints[idx].transform.position) < 0.1f && idx < pathPoints.Count-1)
            {

                yield return new WaitForSeconds(1f);
                idx++;
                heading = pathPoints[idx].transform.position - transform.position;
                direction = heading / heading.magnitude;
                
            } else if (idx >= pathPoints.Count)
            {
                StartCoroutine(FlyStraight(Vector3.left));
            }
                yield return null;
        }
    }
}
