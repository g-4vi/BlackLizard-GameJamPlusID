using UnityEngine;

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

    [Header("Sound Effects")]
    [SerializeField] private SfxID _deathSound;
    [SerializeField] private SfxID _jumpSound;
    [SerializeField] private SfxID _hurtSound;
    [SerializeField] private SfxID _moveSound;

    public SfxID DeathSound => _deathSound;
    public SfxID JumpSound => _jumpSound;
    public SfxID HurtSound => _hurtSound;
    public SfxID MoveSound => _moveSound;

    // Update how decrease health, so it can absorb damage when hasShield is true before reducing health.
    // And when health increased, it does not call hurt sfx.
    // - Thyyn

    public void UpdateHealth(int incrementHealth) {
        if (incrementHealth > 0) {
            health += incrementHealth;
            Debug.Log($"Player healed. Current health: {health}");
        } else {
            if (hasShield) UpdateShield(false); // If has shield, absorb damage first
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

    public void UpdateShield(bool value) {
        hasShield = value;
        onShieldChanged?.Invoke(hasShield);
    }
}
