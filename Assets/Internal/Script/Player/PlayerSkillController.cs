using GameJamPlus.SkillModules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamPlus {
    /// <summary>
    /// Handles player skills and cooldowns.
    /// This script is responsible for managing the player's skills, including executing skills and handling cooldowns
    /// </summary>
    public class PlayerSkillController : MonoBehaviour {

        public System.Action<Skill> OnFixedSkillAssigned;
        public System.Action<Skill> OnSkill1Assigned;

        [SerializeField] Skill fixedSkill;
        public Skill FixedSkill => fixedSkill;
        [SerializeField] Skill skillSlot1;
        public Skill SkillSlot1 => skillSlot1;

        // [SerializeField] SfxID _fireballSkill;

        void Start() {
            if (fixedSkill != null) OnFixedSkillAssigned?.Invoke(fixedSkill);
            if (skillSlot1 != null) OnSkill1Assigned?.Invoke(skillSlot1);
        }

        void Update() {
            if (Time.deltaTime == 0f) return;
            fixedSkill?.Tick(Time.unscaledDeltaTime);
            skillSlot1?.Tick(Time.unscaledDeltaTime);
        }

        #region Input
        public void OnFire(InputValue value) {
            if (!value.isPressed || !fixedSkill.IsReady) { return; }

            if (fixedSkill == null) {
                Debug.LogWarning($"[{name}] No fixed skill assigned to PlayerSkillController.");
                return;
            }

            //Attack animation
            Player player = PlayerManager.Instance.playerInstance;
            player.anim.SetTrigger(player.AttackHash);              // Skill executed in animation event, right ?

            // Moving animation trigger and sfx in skill execution.
            // Sfx moved to Skill.cs ActivateSpell method, the settings in ScriptableObject
            // And maybe for animation trigger, we can also set it in individual skill if needed.
            // - Thyyn

            // TODO: call sound effect when casting skill
            // if (_fireballSkill != SfxID.None) AudioManager.Instance.PlaySFX(_fireballSkill);
        }

        public void OnSubFire(InputValue value) { // For now its right click to use skill in slot 1
            if (!value.isPressed) { return; }

            if (skillSlot1 == null) {
                Debug.LogWarning($"[{name}] No skill assigned to Skill Slot 1.");
                return;
            }

            skillSlot1.ActivateSpell(this.gameObject);
        }
        #endregion

        public void ExecuteSkill() {
            fixedSkill.ActivateSpell(this.gameObject);
        }

        public void AssignSlot1Skill(Skill newSkill) {
            skillSlot1 = newSkill;
            OnSkill1Assigned?.Invoke(skillSlot1);
        }

        void OnDestroy() {
            if (fixedSkill != null) fixedSkill.OnSkillCooldownUpdate = null;
            if (skillSlot1 != null) skillSlot1.OnSkillCooldownUpdate = null;
        }
    }
}