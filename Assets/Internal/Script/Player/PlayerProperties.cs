using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerProperties {
    public System.Action<int> onHealthChanged;
    public System.Action<int> onManaChanged;
    public System.Action<bool> onShieldChanged;

    public int health = 3;
    public float speed = 8f;
    public float jumpForce = 12f;
    [Tooltip("Invisible period after getting damaged")]
    public float invisiblePeriod = 0.5f;
    public int mana = 0;
    public bool hasShield = false;

    GameObject shield;

    [Header("Sound Effects")]
    [SerializeField] private SfxID _deathSound;
    [SerializeField] private SfxID _jumpSound;
    [SerializeField] private SfxID _hurtSound;
    [SerializeField] private SfxID _moveSound;

    [SerializeField] private ItemData itemData;
    public ItemData ItemData => itemData;

    public SfxID DeathSound => _deathSound;
    public SfxID JumpSound => _jumpSound;
    public SfxID HurtSound => _hurtSound;
    public SfxID MoveSound => _moveSound;

    public void UpdateHealth(int incrementHealth) {
        if (incrementHealth > 0) {
            health += incrementHealth;
            Debug.Log($"Player healed. Current health: {health}");
        } else {
            if (hasShield) UpdateShield(null); // If has shield, absorb damage first
            else health += incrementHealth; // otherwise reduce health

            if (health <= 0) { // Game over
                health = 0;
                if (DeathSound != SfxID.None) AudioManager.Instance.PlaySFX(DeathSound);
                GameManager.Instance.EndGame();
                Debug.Log("Game Over!");
                return;
            }

            if (HurtSound != SfxID.None) AudioManager.Instance.PlaySFX(HurtSound);
            Debug.Log($"Player took damage. Current health: {health}");
        }

        onHealthChanged?.Invoke(health);
    }

    public void UpdateMana(int incrementMana) {
        mana += incrementMana;
        onManaChanged?.Invoke(mana);
    }

    public void UpdateShield(GameObject shield) {
        hasShield = shield != null;
        if (this.shield != null) GameObject.Destroy(this.shield);
        this.shield = shield;
        onShieldChanged?.Invoke(hasShield);
    }
}
