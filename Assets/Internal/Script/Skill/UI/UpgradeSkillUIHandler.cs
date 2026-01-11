using GameJamPlus.SkillModules.Common;
using UnityEngine;

namespace GameJamPlus.SkillModules.UI {
    public class UpgradeSkillUIHandler : MonoBehaviour {
        [Header("References")]
        public GameObject skillSlotButtonPrefab;
        public GameObject skillSlotContainer;
        public GameObject upgradeStatusPanel;

        PlayerSkillController playerSkillController;
        bool isInitialized;

        void Start() {
            playerSkillController = PlayerManager.Instance.playerSkillController;
            isInitialized = true;
            OnEnable();
        }

        void OnEnable() {
            if (!isInitialized) return;

            OpenPanel(skillSlotContainer);
            UpdateUpgradeSkillUI();
        }

        /// <summary>
        /// Opens the Upgrade Skill UI.
        /// </summary>
        public void OpenUpgradeSkillUI() {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Closes the Upgrade Skill UI.
        /// </summary>
        public void CloseUpgradeSkillUI() {
            gameObject.SetActive(false);
        }

        void OpenPanel(GameObject panel) {
            CloseAllPanels();
            panel.SetActive(true);
        }

        void CloseAllPanels() {
            skillSlotContainer.SetActive(false);
            upgradeStatusPanel.SetActive(false);
        }

        void UpdateUpgradeSkillUI() {
            foreach (Transform child in skillSlotContainer.transform) {
                Destroy(child.gameObject);
            }

            foreach (var skillSlot in playerSkillController.AllSkillSlots) {
                CreateSkillSlotUI(skillSlot);
            }
        }

        void CreateSkillSlotUI(SkillSlot skillSlot) {
            if (skillSlot == null || skillSlot.asset == null) return;
            var slotObj = Instantiate(skillSlotButtonPrefab, skillSlotContainer.transform);
            var slotUI = slotObj.GetComponent<SkillSlotButton>();
            slotUI.Initialize(skillSlot, OnSlotUIClicked);
        }

        void OnSlotUIClicked(SkillSlot skill) {
            OpenPanel(upgradeStatusPanel);
            var upgradeUI = upgradeStatusPanel.GetComponent<SkillSlotUpgradeStatus>();
            upgradeUI.Initialize(skill, OnBackButtonClicked);
        }

        void OnBackButtonClicked(SkillSlot skill) {
            OpenPanel(skillSlotContainer);
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (skillSlotButtonPrefab != null && skillSlotButtonPrefab.GetComponent<SkillSlotButton>() == null) {
                Debug.LogError($"[{nameof(SelectionSkillUIHandler)}] The assigned skillSlotButtonPrefab does not have a SkillSlotButton component.");
                skillSlotButtonPrefab = null;
            }
        }
#endif
    }
}
