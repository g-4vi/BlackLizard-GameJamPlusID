using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamPlus {
    public class HUDCanvasHandler : MonoBehaviour {

        [Header("Health Settings")]
        [SerializeField] GameObject healthContainer;
        [SerializeField] GameObject healthPrefab;

        [Header("Score Settings")]
        [SerializeField] TMP_Text scoreText;

        [Header("Skill Cooldown Settings")]
        [SerializeField] Image skillCooldownImage;

        bool isInitialized = false;

        void LateUpdate() {
            if (!isInitialized) { // TODO: Player and SkillController references should be cached better, maybe via PlayerManager/GameManager
                var player = FindFirstObjectByType<Player>();
                var skillController = player.GetComponent<PlayerSkillController>();

                player.playerProperties.onHealthChanged += UpdateVisualHealth;
                player.playerProperties.onManaChanged += UpdateVisualMana;
                skillController.onSkillCooldownUpdate += UpdateVisualSkillCooldown;

                UpdateVisualHealth(player.playerProperties.health);
                UpdateVisualMana(player.playerProperties.mana);
                UpdateVisualSkillCooldown(1f, 1f);

                isInitialized = true;
            }
        }

        public void UpdateVisualHealth(int health) {
            foreach (Transform child in healthContainer.transform) {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < health; i++) {
                Instantiate(healthPrefab, healthContainer.transform);
            }
        }

        public void UpdateVisualMana(int score) {
            scoreText.text = score.ToString();
        }

        public void UpdateVisualSkillCooldown(float cooldown, float maxCooldown) {
            float normalizedCooldown = Mathf.Clamp01(cooldown / maxCooldown);
            skillCooldownImage.fillAmount = normalizedCooldown;
            if (normalizedCooldown <= 0.01f) skillCooldownImage.fillAmount = 1f;
        }

    }
}