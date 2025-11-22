using System;
using UnityEngine;


public abstract class ObstacleProperties : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] protected float _objectSpeed = 5f;
    [SerializeField] protected float _objectHealth = 1f;
    [SerializeField] protected float _objectDamage = 1f;
}
