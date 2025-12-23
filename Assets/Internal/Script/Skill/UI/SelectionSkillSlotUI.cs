using System;
using GameJamPlus.SkillModules.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameJamPlus.SkillModules.UI {
    /// <summary>
    /// UI component for a single skill slot in the skill selection UI.
    /// Need SelectionSkillUIHandler to function properly.
    /// </summary>
    public class SelectionSkillSlotUI : MonoBehaviour {
        Button slotButton;
        Image skillIconImage;
        TMP_Text skillNameText;

        BaseSkill skill;
        UnityAction<BaseSkill> callback;

        void Awake() {
            if (slotButton == null) slotButton = GetComponentInChildren<Button>();
            if (skillIconImage == null) skillIconImage = GetComponentInChildren<Image>();
            if (skillNameText == null) skillNameText = GetComponentInChildren<TMP_Text>();
        }

        public void Initialize(BaseSkill skill, UnityAction<BaseSkill> callback) {
            this.skill = skill;
            this.callback = callback;
            UpdateUI(skill);
            slotButton.onClick.AddListener(OnClicked);
        }

        void OnClicked() {
            callback?.Invoke(skill);
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

        void OnDisable() {
            slotButton.onClick.RemoveAllListeners();
        }

    }
}