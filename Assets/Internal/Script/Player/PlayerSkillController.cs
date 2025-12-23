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
            fixedSkill?.asset?.Tick(fixedSkill, dt);
            skillSlot1?.asset?.Tick(skillSlot1, dt);
        }

        #region Input
        public void OnFire(InputValue value) {
            if (!value.isPressed) return;

            if (fixedSkill?.asset == null) return;

            //Attack animation and call skill execution
            Player player = PlayerManager.Instance.playerInstance;
            player.anim.SetTrigger(player.AttackHash);
        }

        public void OnSubFire(InputValue value) { // For now its right click to use skill in slot 1
            if (!value.isPressed) { return; }

            skillSlot1?.asset?.ActivateSpell(gameObject, skillSlot1);
        }
        #endregion

        public void ExecuteSkill() {
            fixedSkill?.asset?.ActivateSpell(gameObject, fixedSkill);
        }

        public void AssignSlot1Skill(BaseSkill newSkill) {
            skillSlot1 = new SkillSlot { asset = newSkill, level = 1, cooldownTimer = 0f };
            OnSkill1Assigned?.Invoke(newSkill);
        }

        void OnDestroy() {
            OnSkill1Assigned = null;
        }
    }
}