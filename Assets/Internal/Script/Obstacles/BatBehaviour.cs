using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatBehaviour : ObstacleProperties
{
    [Header("References")]

    [SerializeField] List<Vector3> _pathPoints;

    protected override void Start()
    {
        base.Start();
        int pattern = Random.Range(0, 10);
        StartCoroutine(FlyFixedPath(_pathPoints));
    }

    public void EditPathPoint(List<Vector3> newPathPoints)
    {
        _pathPoints = newPathPoints;
    }

    IEnumerator FlyStraight(Vector3 direction)
    {
        // TODO: play fly Animation
        while (true)
        {
            transform.position = new Vector3(transform.position.x + direction.x * _objectSpeed * Time.deltaTime,
                                             transform.position.y + direction.y * _objectSpeed * Time.deltaTime,
                                             transform.position.z + direction.z * _objectSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FlyFixedPath(List<Vector3> pathPoints)
    {
        int idx = 0;

        Vector3 heading = pathPoints[idx] - transform.position;
        Vector3 direction = heading / heading.magnitude;
        // TODO: play fly Animation
        while (true)
        {
            transform.Translate(direction*_objectSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pathPoints[idx]) < 0.1f && idx < pathPoints.Count-1)
            {

                yield return new WaitForSeconds(1f);
                idx++;
                heading = pathPoints[idx] - transform.position;
                direction = heading / heading.magnitude;
                
            } else if (idx >= pathPoints.Count)
            {
                StartCoroutine(FlyStraight(Vector3.left));
            }
                yield return null;
        }
    }
}
