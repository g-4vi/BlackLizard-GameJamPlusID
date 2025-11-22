using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Collider2D _objectCollider;
    Animator _objectAnimator;

    SpriteRenderer _sprite;

    void Awake()
    {
        _objectCollider = GetComponent<Collider2D>();
        _objectAnimator = GetComponent<Animator>();
        _objectCollider.enabled = false;
    }

    public void Appear()
    {
        // Tremor Effect

        // Animate the spike trap appearing
        _sprite.enabled = true; // temporary
        _objectCollider.enabled = true;
    }

    public void Disappear()
    {
        // Animate the spike trap disappearing
        _sprite.enabled = false; // temporary
        _objectCollider.enabled = false;
        Destroy(this.gameObject);
    }
}
