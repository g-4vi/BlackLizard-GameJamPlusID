using GameJamPlus.SkillModules.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameJamPlus.SkillModules.UI {
    /// <summary>
    /// Represents a UI button for a skill slot.
    /// </summary>
    public class SkillSlotButton : MonoBehaviour {
        Button slotButton;
        Image skillIconImage;
        TMP_Text skillNameText;

        BaseSkill skill;
        UnityAction<BaseSkill> callback;

        SkillSlot skillSlot;
        UnityAction<SkillSlot> slotCallback;

        void Awake() {
            if (slotButton == null) slotButton = GetComponentInChildren<Button>();
            if (skillIconImage == null) skillIconImage = GetComponentInChildren<Image>();
            if (skillNameText == null) skillNameText = GetComponentInChildren<TMP_Text>();
        }

        void OnEnable() {
            slotButton.onClick.AddListener(OnClicked);
        }

        /// <summary>
        /// Initializes the skill slot button with the given skill and callback.
        /// </summary>
        public void Initialize(BaseSkill skill, UnityAction<BaseSkill> callback) {
            this.skill = skill;
            this.callback = callback;
            UpdateUI(skill);
        }

        /// <summary>
        /// [Overload]
        /// Initializes the skill slot button with the given skill slot and callback.
        /// </summary>
        public void Initialize(SkillSlot skillSlot, UnityAction<SkillSlot> callback) {
            this.skillSlot = skillSlot;
            this.slotCallback = callback;
            Initialize(skillSlot.asset, null);
        }

        void UpdateUI(BaseSkill skill) {
            if (skill != null) {
                gameObject.SetActive(true);
                skillIconImage.sprite = skill.SkillIcon;
                skillNameText.text = skill.SkillName;
            } else {
                gameObject.SetActive(false);
            }
        }

        void OnClicked() {
            callback?.Invoke(skill);
            slotCallback?.Invoke(skillSlot);
        }

        void OnDisable() {
            slotButton.onClick.RemoveAllListeners();
        }

    }
}