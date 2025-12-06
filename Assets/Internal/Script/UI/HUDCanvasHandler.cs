using GameJamPlus.SkillModules.Common;
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
        [SerializeField] Image skill1CooldownImage;

        Player player;
        PlayerSkillController skillController;

        bool _isInitialized = false;

        void Start() {
            player = PlayerManager.Instance.playerInstance;
            skillController = player.GetComponent<PlayerSkillController>();
            skillController.OnSkill1Assigned += OnSkill1Assigned;
            skillController.OnFixedSkillAssigned += OnFixedSkillAssigned;
            skillController.FixedSkill.OnSkillCooldownUpdate += UpdateVisualFixedSkillCooldown;
            _isInitialized = true;
            OnEnable();
        }

        void OnEnable() {
            if (!_isInitialized) return;
            if (player == null || skillController == null) return;

            player.playerProperties.onHealthChanged += UpdateVisualHealth;
            player.playerProperties.onManaChanged += UpdateVisualMana;

            if (skillController.SkillSlot1 != null)
                skillController.SkillSlot1.OnSkillCooldownUpdate += UpdateVisualSkill1Cooldown;

            UpdateVisualHealth(player.playerProperties.health);
            UpdateVisualMana(player.playerProperties.mana);
            UpdateVisualFixedSkillCooldown(1f, 1f);
            UpdateVisualSkill1Cooldown(1f, 1f);
        }
        void OnDisable() {
            if (player == null || skillController == null) return;

            player.playerProperties.onHealthChanged -= UpdateVisualHealth;
            player.playerProperties.onManaChanged -= UpdateVisualMana;

            if (skillController.SkillSlot1 != null)
                skillController.SkillSlot1.OnSkillCooldownUpdate -= UpdateVisualSkill1Cooldown;
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

        void OnFixedSkillAssigned(Skill skill) {
            skill.OnSkillCooldownUpdate += UpdateVisualFixedSkillCooldown;
            UpdateVisualFixedSkillCooldown(1f, 1f);
            // skillCooldownImage.sprite = skill.SkillIcon; // TODO : change the icon ?
        }

        void OnSkill1Assigned(Skill skill) {
            skill.OnSkillCooldownUpdate += UpdateVisualSkill1Cooldown;
            UpdateVisualSkill1Cooldown(1f, 1f);
            // skill1CooldownImage.sprite = skill.SkillIcon;
        }

        void UpdateVisualFixedSkillCooldown(float cooldown, float maxCooldown) {
            UpdateVisualSkillCooldown(skillCooldownImage, cooldown, maxCooldown);
        }

        void UpdateVisualSkill1Cooldown(float cooldown, float maxCooldown) {
            UpdateVisualSkillCooldown(skill1CooldownImage, cooldown, maxCooldown);
        }

        void UpdateVisualSkillCooldown(Image target, float cooldown, float maxCooldown) {
            float normalizedCooldown = Mathf.Clamp01(cooldown / maxCooldown);
            target.fillAmount = normalizedCooldown;
            if (normalizedCooldown <= 0.01f) target.fillAmount = 1f;
        }

        void OnDestroy() {
            skillController.OnSkill1Assigned -= OnSkill1Assigned;
            skillController.OnFixedSkillAssigned -= OnFixedSkillAssigned;
            skillController.FixedSkill.OnSkillCooldownUpdate -= UpdateVisualFixedSkillCooldown;
        }

    }
}