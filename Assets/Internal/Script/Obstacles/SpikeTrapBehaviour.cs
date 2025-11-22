using System.Collections;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] private float _persistTime = 2f;
    Collider2D _objectCollider;
    Animator _objectAnimator;

    SpriteRenderer _sprite;

    void Awake()
    {
        _objectCollider = GetComponent<Collider2D>();
        _objectAnimator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = true;
        _objectCollider.enabled = false;
        StartCoroutine(Appear());
    }


    IEnumerator Appear()
    {
        //Tremor Effect
        _sprite.enabled = true; // temporary
        _objectCollider.enabled = true;
        //Play animation

        yield return new WaitForSeconds(_persistTime);

        _sprite.enabled = false; // temporary
        _objectCollider.enabled = false;
        Destroy(this.gameObject);
    }


}
