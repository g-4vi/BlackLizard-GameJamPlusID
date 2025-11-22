using UnityEngine;

namespace GameJamPlus.ManaModules {
    public class ManaItem : MonoBehaviour {

        public System.Action OnCollected;

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent<PlayerController>(out var player)) {
                // TODO: Add score to player
                // and maybe play a sound effect or visual effect

                OnCollected?.Invoke();
                Destroy(gameObject);

                Debug.Log($"[{name}] Mana collected by Player");
            }
        }

    }
}
