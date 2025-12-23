using System.Security.Cryptography.X509Certificates;
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
                slotUI.Initialize(this, skill);
            }
        }

        public void CloseSkillSelectionUI() {
            this.gameObject.SetActive(false);
        }

        // this method is called by SelectionSkillSlotUI when a slot is selected
        public void OnSkillSlotSelected(BaseSkill skill) {
            _playerSkillController.AssignSlot1Skill(skill);
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