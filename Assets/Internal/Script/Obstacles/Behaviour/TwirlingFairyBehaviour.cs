using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TwirlingFairyBehaviour : ObstacleProperties
{
    [SerializeField] private float _attackDelay = 2f;
    [SerializeField] float _attackDuration = 1f;
    [SerializeField] private int _numOfAttacks = 3;
    [SerializeField] private float _attackColliderWidth = 3f;

    BoxCollider2D _objectCollider;
    Animator _objectAnimator;

    SpriteRenderer _sprite;

    void Awake()
    {
        _objectCollider = GetComponent<BoxCollider2D>();
        _objectAnimator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = true;
        _objectCollider.enabled = false;
        StartCoroutine(AttackRoutine());
    }


    IEnumerator AttackRoutine()
    {
        //Tremor Effect
        _sprite.enabled = true; // temporary
        _objectCollider.enabled = true;
        //Play animation
        float initialColliderWidth = _objectCollider.size.x;
        for (int i = 0; i < _numOfAttacks; i++)
        {
            _objectAnimator.SetTrigger("Attack");
            _objectCollider.size = new Vector2(_attackColliderWidth, _objectCollider.size.y);
            yield return new WaitForSeconds(_attackDuration); 
            _objectCollider.size = new Vector2(initialColliderWidth, _objectCollider.size.y);
            yield return new WaitForSeconds(_attackDelay); 
        }

        _sprite.enabled = false; // temporary
        _objectCollider.enabled = false;
        _objectAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f); // insert exit animation duration
        Destroy(this.gameObject);
    }
}
