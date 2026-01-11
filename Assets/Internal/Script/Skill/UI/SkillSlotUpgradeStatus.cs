using System;
using GameJamPlus.SkillModules.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJamPlus.SkillModules.UI {
    /// <summary>
    /// Handles the UI display for skill slot upgrade status.
    /// </summary>
    public class SkillSlotUpgradeStatus : MonoBehaviour {

        public enum StatPolarity {
            HigherIsBetter, // For stats where a higher value is an improvement
            LowerIsBetter   // For stats where a lower value is an improvement
        }

        [Header("Skill Info References")]
        public Image skillIconImage;
        public TMP_Text skillNameText;
        public TMP_Text skillLevelText;

        [Header("Current Stats References")]
        public TMP_Text coolddownText;
        public TMP_Text durationText;
        public TMP_Text manaCostText;

        [Header("Next Level Stats References")]
        public TMP_Text nextCooldownText;
        public TMP_Text nextDurationText;
        public TMP_Text nextManaCostText;

        [Header("Upgrade Button Reference")]
        public TMP_Text upgradeCostText;
        public Button upgradeButton;
        public Button backButton;

        SkillSlot skill;
        Action<SkillSlot> backCallback;

        PlayerInventory playerInventory => PlayerInventory.Instance;

        void OnEnable() {
            WireNewEvents();
        }

        /// <summary>
        /// Initializes the skill slot upgrade status UI with the given skill and back callback.
        /// </summary>
        public void Initialize(SkillSlot skill, Action<SkillSlot> backCallback) {
            this.skill = skill;
            this.backCallback = backCallback;
            UpdateUI(skill);
        }

        // Updates the UI elements based on the provided skill slot data.
        void UpdateUI(SkillSlot skill) {
            if (skill == null) return;

            skillIconImage.sprite = skill.asset.SkillIcon;
            skillNameText.text = skill.asset.SkillName;
            SetStatPreviewWithArrow(skillLevelText, skill.level, skill.level + 1, showDiff: false);

            var currentData = skill.asset.Progression.GetLevel(skill.level);
            SetStatPreview(coolddownText, currentData.cooldown, "s");
            SetStatPreview(durationText, currentData.duration, "s");
            SetStatPreview(manaCostText, currentData.manaCost);

            var nextData = skill.asset.Progression.GetLevel(skill.level + 1);
            SetStatPreviewWithArrow(nextCooldownText, currentData.cooldown, nextData.cooldown, StatPolarity.LowerIsBetter, "s");
            SetStatPreviewWithArrow(nextDurationText, currentData.duration, nextData.duration, StatPolarity.HigherIsBetter, "s");
            SetStatPreviewWithArrow(nextManaCostText, currentData.manaCost, nextData.manaCost);

            upgradeCostText.text = $"{nextData.upgradeCost}";
        }

        // Helper methods to set text with or without arrows
        void SetStatPreview(TMP_Text text, float value, string suffix = "") {
            text.text = $"{value}{suffix}";
        }

        // Helper method to set text with an arrow indicating improvement or not
        void SetStatPreviewWithArrow(TMP_Text text, float cur, float next, StatPolarity statPolarity = StatPolarity.HigherIsBetter, string suffix = "", bool showDiff = true) {
            if (Math.Abs(cur - next) < 0.01f) {
                text.text = $"{cur}{suffix}";
                return;
            }

            bool isImprovement = statPolarity == StatPolarity.HigherIsBetter ? next > cur : next < cur;
            string arrow = statPolarity == StatPolarity.HigherIsBetter ? "▲" : "▼";
            string colorTagStart = isImprovement ? "<color=green>" : "<color=red>";
            string colorTagEnd = "</color>";

            if (showDiff) {
                float valueDiff = Math.Abs(next - cur);
                text.text = $"{next}{suffix} {colorTagStart}{arrow} {valueDiff}{suffix}{colorTagEnd}";
            } else {
                text.text = $"{cur}{suffix} {colorTagStart}{arrow} {next}{suffix}{colorTagEnd}";
            }
        }

        void WireNewEvents() {
            upgradeButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        void OnUpgradeButtonClicked() {
            SkillUpgradeService.LevelUp(skill, playerInventory);
            UpdateUI(skill);
        }

        void OnBackButtonClicked() {
            backCallback?.Invoke(skill);
        }

    }
}