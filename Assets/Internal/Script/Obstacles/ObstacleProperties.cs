using System;
using UnityEngine;


public abstract class ObstacleProperties : MonoBehaviour
{
    public System.Action<float> onHealthChanged;

    [Header("Properties")]
    [SerializeField] protected float _objectSpeed = 5f;
    [SerializeField] protected float _objectHealth = 1f;
    [SerializeField] protected int _objectDamage = 1;
    protected Vector3 _direction;
    protected ObstacleSlot _obstacleSlot;

    [Header("Knockback")]
    [SerializeField] protected float knockbackForce = 5f;
    [SerializeField] protected float knockbackDuration = 0.2f;

    [Header("Sound Effects")]
    [SerializeField] protected SfxID _destroyedSound;
    [SerializeField] protected SfxID _entrySound;

    [Tooltip("Unique Sound Effects (I.e Spider Shooting)")]
    [SerializeField] protected SfxID _specialSound; // spider shooting sound, etc.

    [Header("Animation")]
    [SerializeField] protected Animator _animator;


    public void UpdateHealth(int incrementHealth) {
        _objectHealth += incrementHealth;

        if (_objectHealth <= 0)//Game over
        {
            _objectHealth = 0;
            DestroyObstacle();
            return;
        }

        onHealthChanged?.Invoke(_objectHealth);
    }

    protected virtual void Start()
    {
        if (_entrySound != SfxID.None) AudioManager.Instance.PlaySFX(_entrySound);

    }

    protected void Update()
    {
        if (OutOfScreen())
        {
            Destroy(this.gameObject);
        }
    }

    bool OutOfScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.3 || screenPoint.x > 1.3 || screenPoint.y < -0.3 || screenPoint.y > 1.3;
    }

    void DestroyObstacle()
    {
        // TODO: Play destroy anim
        // TODO: Play destroy SFX
        AudioManager.Instance.StopSFX(_entrySound);
        ;
        if (_destroyedSound != SfxID.None) AudioManager.Instance.PlaySFX(_destroyedSound);


        if (_obstacleSlot != null) _obstacleSlot.ChangeOccupyStatus(false);
        Destroy(this.gameObject);
    }


    void DealDamageToPlayer(int damage)
    {
        // Access Player Health Component
        // Subtract Damage from Player Health
        Player player = PlayerManager.Instance.playerInstance;
        if (!player.IsInvisible)
        {
            Debug.Log("Ouch! GOt hit");
            PlayerManager.Instance.TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))//for boulders
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Debug.Log("Fireball hit!");
            Destroy(collision.gameObject);
            UpdateHealth(-1);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit obstacle!");
            DealDamageToPlayer(_objectDamage);
            Collider2D[] playerColliders = collision.GetComponentsInParent<Collider2D>();
            foreach (var col in playerColliders)
                Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());

            Transform player = collision.gameObject.transform;
            Vector2 direction = (player.position - transform.position).normalized;//direction of obstacle, + is from left
            direction = new Vector2(Mathf.Sign(direction.x), 0);//only horizontal knockback
            player.GetComponent<PlayerMovement>().OnDamaged(direction, knockbackForce, knockbackDuration);

            if (gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        _direction = newDirection;
    }
    public void AssignSlot(ObstacleSlot slotObj)
    {
        _obstacleSlot = slotObj;
    }
}
