using UnityEngine;

[System.Serializable]
public class PlayerProperties {
    public System.Action<int> onHealthChanged;
    public System.Action<int> onManaChanged;

    public int health = 3;
    public float speed = 8f;
    public float jumpForce = 12f;
    [Tooltip("Invisible period after getting damaged")]
    public float invisiblePeriod = 0.5f;
    public int mana = 0;

    [Header("Sound Effects")]
    [SerializeField] private SfxID _deathSound;
    [SerializeField] private SfxID _jumpSound;
    [SerializeField] private SfxID _hurtSound;
    [SerializeField] private SfxID _moveSound;

    public SfxID DeathSound => _deathSound;
    public SfxID JumpSound => _jumpSound;
    public SfxID HurtSound => _hurtSound;
    public SfxID MoveSound => _moveSound;


    public void UpdateHealth(int incrementHealth) {
        health += incrementHealth;

        if (health <= 0)//Game over
        {
            health = 0;
            GameManager.Instance.EndGame();
            if (DeathSound != SfxID.None) AudioManager.Instance.PlaySFX(DeathSound);
            Debug.Log("Game Over!");
            return;
        }
        if (HurtSound != SfxID.None) AudioManager.Instance.PlaySFX(HurtSound);
        onHealthChanged?.Invoke(health);
    }

    public void UpdateMana(int incrementMana) {
        mana += incrementMana;
        onManaChanged?.Invoke(mana);
    }
}
