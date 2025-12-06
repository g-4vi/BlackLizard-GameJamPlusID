using TMPro;
using UnityEngine;
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

        SelectionSkillUIHandler _selector;
        Common.Skill _assignedSkill;

        void Awake() {
            if (slotButton == null) slotButton = GetComponentInChildren<Button>();
            if (skillIconImage == null) skillIconImage = GetComponentInChildren<Image>();
            if (skillNameText == null) skillNameText = GetComponentInChildren<TMP_Text>();
        }

        public void Initialize(SelectionSkillUIHandler sel, Common.Skill skill) {
            _selector = sel;
            _assignedSkill = skill;

            UpdateUI();
            slotButton.onClick.AddListener(OnSlotButtonClicked);
        }

        void UpdateUI() {
            if (_assignedSkill != null) {
                skillIconImage.sprite = _assignedSkill.SkillIcon;
                skillNameText.text = _assignedSkill.SkillName;
            } else {
                Destroy(this.gameObject);
            }
        }

        void OnSlotButtonClicked() {
            _selector.OnSkillSlotSelected(_assignedSkill);
            _selector.CloseSkillSelectionUI();
        }

        void OnDestroy() {
            slotButton.onClick.RemoveListener(OnSlotButtonClicked);
        }

    }
}