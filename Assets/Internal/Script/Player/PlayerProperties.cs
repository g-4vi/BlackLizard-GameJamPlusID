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

    public void UpdateHealth(int incrementHealth) {
        health += incrementHealth;
        onHealthChanged?.Invoke(health);
    }

    public void UpdateMana(int incrementMana) {
        mana += incrementMana;
        onManaChanged?.Invoke(mana);
    }
}
