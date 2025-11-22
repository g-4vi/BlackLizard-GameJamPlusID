using UnityEngine;

namespace GameJamPlus.ManaModules {
    public class ManaItem : MonoBehaviour {

        public System.Action OnCollected;

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent<Player>(out var player)) {
                player.playerProperties.UpdateMana(1);
                // TODO: Add sound effect or visual effect here

                OnCollected?.Invoke();
                Destroy(gameObject);

                Debug.Log($"[{name}] Mana collected by Player");
            }
        }

    }
}
