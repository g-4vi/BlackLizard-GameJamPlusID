using GameJamPlus.SkillModules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamPlus {
    /// <summary>
    /// Handles player skills and cooldowns.
    /// This script is responsible for managing the player's skills, including executing skills and handling cooldowns
    /// </summary>
    public class PlayerSkillController : MonoBehaviour {
        public System.Action<BaseSkill> OnSkill1Assigned;

        [Header("Skill Slots")]
        [SerializeField] SkillSlot fixedSkill;
        public SkillSlot FixedSkill => fixedSkill;
        [SerializeField] SkillSlot skillSlot1;
        public SkillSlot SkillSlot1 => skillSlot1;

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
            skillSlot1 = new SkillSlot { asset = newSkill, level = 1, cooldownTimer = 0f };
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

        void OnDestroy() {
            OnSkill1Assigned = null;
        }
    }
}