using UnityEngine;

public class BreakableMapObject : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] protected SfxID _destroyedSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Debug.Log("Fireball hit!");
            Destroy(collision.gameObject);
            DestroyObstacle();
        }
    }

    void DestroyObstacle()
    {
        // TODO: Play destroy anim
        // TODO: Play destroy SFX
        ;
        if (_destroyedSound != SfxID.None) AudioManager.Instance.PlaySFX(_destroyedSound);


        Destroy(this.gameObject);
    }
}
