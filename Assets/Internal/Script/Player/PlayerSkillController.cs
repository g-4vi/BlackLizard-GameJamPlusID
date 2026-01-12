using System.Collections.Generic;
using GameJamPlus.SkillModules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamPlus {
    /// <summary>
    /// Handles player skills and cooldowns.
    /// This script is responsible for managing the player's skills, including executing skills and handling cooldowns
    /// </summary>
    public class PlayerSkillController : MonoBehaviour, IDataPersistence {
        public System.Action<BaseSkill> OnSkill1Assigned;

        [Header("Skill Slots")]
        [SerializeField] SkillSlot fixedSkill;
        public SkillSlot FixedSkill => fixedSkill;
        [SerializeField] SkillSlot skillSlot1;
        public SkillSlot SkillSlot1 => skillSlot1;

        [Header("References")]
        [SerializeField] SkillDatabase skillDatabase;

        public List<SkillSlot> AllSkillSlots { get; private set; } = new List<SkillSlot>();

        void Awake() {
            if (AllSkillSlots.Count != 0 || skillDatabase == null) return;
            foreach (var skill in skillDatabase.allSkills) {
                AllSkillSlots.Add(new SkillSlot { asset = skill, level = 1 });
            }
        }

        void Update() {
            if (Time.deltaTime == 0f) return;
            float dt = Time.unscaledDeltaTime;
            fixedSkill?.Tick(dt);
            skillSlot1?.Tick(dt);
        }

        #region Input
        public void OnFire(InputValue value) {
            if (!value.isPressed) return;

            if (fixedSkill?.asset == null) return;

            //Attack animation and call skill execution
            Player player = PlayerManager.Instance.playerInstance;
            player.anim.SetTrigger(player.AttackHash);
        }

        public void OnSubFire(InputValue value) { // See InputAction for the binding
            if (!value.isPressed) { return; }

            skillSlot1?.ActivateSpell(gameObject);
        }
        #endregion

        public void ExecuteSkill() {
            fixedSkill?.ActivateSpell(gameObject);
        }

        public void AssignSlot1Skill(BaseSkill newSkill) {
            skillSlot1 = AllSkillSlots.Find(slot => slot.asset == newSkill);
            OnSkill1Assigned?.Invoke(newSkill);
        }

        #region Skill Upgrade Service Methods
        public void DoUpgradeFixedSkill(PlayerInventory resource) {
            SkillUpgradeService.LevelUp(fixedSkill, resource);
        }

        public void DoUpgradeSkillSlot1(PlayerInventory resource) {
            SkillUpgradeService.LevelUp(skillSlot1, resource);
        }
        #endregion

        #region Skill Slot Data Persistence
        public void LoadData(GameData gameData) {
            Awake(); // ensure AllSkillSlots is initialized

            var skillSlots = gameData.playerSkillData.skillSlots;

            // update existing skill slots or if not found (should not happen), add new ones
            foreach (var slotData in skillSlots) {
                SkillSlot slotInController = AllSkillSlots.Find(s => s.asset == slotData.asset);
                if (slotInController != null) {
                    slotInController.level = slotData.level;
                } else {
                    AllSkillSlots.Add(new SkillSlot {
                        asset = slotData.asset,
                        level = slotData.level,
                    });
                }
            }
        }

        public void SaveData(GameData gameData) {
            gameData.playerSkillData.UpdateSkillSlot(fixedSkill, fixedSkill.level);
            gameData.playerSkillData.UpdateSkillSlot(skillSlot1, skillSlot1.level);
        }
        #endregion

        void OnDestroy() {
            OnSkill1Assigned = null;
        }
    }
}