using UnityEngine;

namespace GameJamPlus.ManaModules {
    public class ManaItem : MonoBehaviour {

        public System.Action OnCollected;
        [SerializeField] SfxID _collectSFX;


        bool isCollected;


        void OnTriggerEnter2D(Collider2D collision) {
            if (isCollected) return;
            if (collision.TryGetComponent<Player>(out var player)) {
                isCollected = true;

                player.playerProperties.UpdateMana(1);

                // TODO: Add sound effect or visual effect here
                if(_collectSFX != SfxID.None) AudioManager.Instance.PlaySFX(_collectSFX);

                OnCollected?.Invoke();
                OnCollected = null;
                Debug.Log($"[{name}] Mana collected by Player");

                Destroy(gameObject);
            }
        }

    }
}
