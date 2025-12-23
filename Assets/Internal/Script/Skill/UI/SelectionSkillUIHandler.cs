using GameJamPlus.SkillModules.Common;
using UnityEngine;

namespace GameJamPlus.SkillModules.UI {
    /// <summary>
    /// UI handler for the skill selection interface.
    /// Manages displaying available skills and handling skill selection.
    /// </summary>
    public class SelectionSkillUIHandler : MonoBehaviour {

        [Header("UI Settings")]
        [SerializeField] GameObject skillSlotPrefab;
        [SerializeField] Transform skillSlotsContainer;
        [SerializeField] SkillDatabase skillDatabase;

        PlayerSkillController _playerSkillController;

        bool _isInitialized = false;

        void Start() {
            _playerSkillController = PlayerManager.Instance.playerSkillController;
            _isInitialized = true;
            OnEnable();
        }

        void OnEnable() {
            if (!_isInitialized) return;
            UpdateSkillSlots();
        }

        void UpdateSkillSlots() {
            foreach (Transform child in skillSlotsContainer) {
                Destroy(child.gameObject);
            }

            foreach (var skill in skillDatabase.allSkills) {
                if (skill != null && skill == _playerSkillController.FixedSkill.asset) continue;
                var slotObj = Instantiate(skillSlotPrefab, skillSlotsContainer);
                var slotUI = slotObj.GetComponent<SelectionSkillSlotUI>();
                slotUI.Initialize(skill, OnSkillSlotSelected);
            }
        }

        public void OpenSkillSelectionUI() {
            gameObject.SetActive(true);
        }

        public void CloseSkillSelectionUI() {
            gameObject.SetActive(false);
        }

        public void OnSkillSlotSelected(BaseSkill selectedSkill) {
            _playerSkillController.AssignSlot1Skill(selectedSkill);
            CloseSkillSelectionUI();
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (skillSlotPrefab != null && skillSlotPrefab.GetComponent<SelectionSkillSlotUI>() == null) {
                Debug.LogError($"[{nameof(SelectionSkillUIHandler)}] The assigned skillSlotPrefab does not have a SelectionSkillSlotUI component.");
                skillSlotPrefab = null;
            }
        }
#endif

    }
}