using GameJamPlus.SkillModules.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamPlus {
    public class HUDInGameHandler : MonoBehaviour {

        [Header("Health Settings")]
        [SerializeField] GameObject healthContainer;
        [SerializeField] GameObject healthPrefab;

        [Header("Score Settings")]
        [SerializeField] TMP_Text scoreText;

        [Header("Skill Cooldown Settings")]
        [SerializeField] Image skillCooldownImage;
        [SerializeField] Image skill1CooldownImage;

        Player player;
        PlayerSkillController skillController;

        SkillSlot fixedSkill;
        SkillSlot skillSlot1;

        bool _isInitialized = false;

        void Start() {
            player = PlayerManager.Instance.playerInstance;
            skillController = player.GetComponent<PlayerSkillController>();

            fixedSkill = skillController.FixedSkill;
            if (fixedSkill?.asset != null) skillCooldownImage.sprite = fixedSkill.asset.SkillIcon;

            skillSlot1 = skillController.SkillSlot1;
            skillController.OnSkill1Assigned += UpdateSkillIcons;

            _isInitialized = true;
            OnEnable();
        }

        void OnEnable() {
            if (!_isInitialized) return;
            if (player == null || skillController == null) return;

            player.playerProperties.onHealthChanged += UpdateVisualHealth;
            player.playerProperties.onManaChanged += UpdateVisualMana;
            UpdateVisualHealth(player.playerProperties.health);
            UpdateVisualMana(player.playerProperties.mana);

            UpdateSkillIcons(skillSlot1?.asset);
            skillCooldownImage.fillAmount = 1f;
            if (skill1CooldownImage != null) skill1CooldownImage.fillAmount = 1f;
        }

        void Update() {
            if (Time.deltaTime == 0f) return;

            UpdateCooldownVisual(fixedSkill, skillCooldownImage);
            UpdateCooldownVisual(skillSlot1, skill1CooldownImage);
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

        void UpdateSkillIcons(BaseSkill newSkill) {
            if (skill1CooldownImage == null) return;

            skillSlot1 = skillController.SkillSlot1;
            if (skillSlot1?.asset != null) {
                skill1CooldownImage.gameObject.SetActive(true);
                skill1CooldownImage.sprite = skillSlot1.asset.SkillIcon;
            } else {
                skill1CooldownImage.gameObject.SetActive(false);
            }
        }

        void UpdateCooldownVisual(SkillSlot slot, Image img) {
            if (img == null) return;
            if (slot == null || slot.asset == null) {
                img.fillAmount = 1f;
                return;
            }

            var data = slot.asset.Progression.GetLevel(slot.level);
            float maxCooldown = data.cooldown;

            if (maxCooldown <= 0f) {
                img.fillAmount = 1f;
                return;
            }

            float normalized = Mathf.Clamp01(1f - (slot.cooldownTimer / maxCooldown));
            img.fillAmount = normalized > 0f ? normalized : 1f;
        }

        void OnDisable() {
            if (player == null || skillController == null) return;

            player.playerProperties.onHealthChanged -= UpdateVisualHealth;
            player.playerProperties.onManaChanged -= UpdateVisualMana;
        }

    }
}