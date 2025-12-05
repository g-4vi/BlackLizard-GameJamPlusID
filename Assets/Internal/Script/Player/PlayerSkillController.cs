using GameJamPlus.SkillModules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamPlus {
    /// <summary>
    /// Handles player skills and cooldowns.
    /// This script is responsible for managing the player's skills, including executing skills and handling cooldowns
    /// </summary>
    public class PlayerSkillController : MonoBehaviour {

        public System.Action<float, float> onSkillCooldownUpdate;

        [SerializeField] Skill currentSkill;

        [SerializeField] SfxID _fireballSkill;

        float currentSkillCooldown;

        void Update() {
            if (currentSkillCooldown > 0f) {
                if (Time.deltaTime <= 0f) return; // Pause check
                currentSkillCooldown -= Time.unscaledDeltaTime;
                onSkillCooldownUpdate?.Invoke(currentSkillCooldown, currentSkill.cooldown);
            }
        }

        public void OnFire(InputValue value) {
            if (!value.isPressed) { return; }

            if (currentSkill == null) {
                Debug.LogWarning($"[{name}] No skill assigned to PlayerSkillController.");
                return;
            }

            if (currentSkillCooldown <= 0f) {
                Debug.Log($"[{name}] Attack input detected, executing current skill.");
                // ExecuteSkill();

                //Attack animation
                Player player = PlayerManager.Instance.playerInstance;
                player.anim.SetTrigger(player.AttackHash);              // Skill executed in animation event, right ?

                currentSkillCooldown = currentSkill.cooldown;
                // TODO: call sound effect when casting skill
                if (_fireballSkill != SfxID.None) AudioManager.Instance.PlaySFX(_fireballSkill);

            } else {
                Debug.Log($"[{name}] Skill is on cooldown for {currentSkillCooldown} more seconds.");
            }
        }

        public void ExecuteSkill() {
            currentSkill.ActivateSpell(this.gameObject);
        }

        void OnDestroy() {
            onSkillCooldownUpdate = null;
        }
    }
}