using GameJamPlus.SkillModules.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamPlus {
    public class PlayerSkillController : MonoBehaviour {

        public System.Action<float, float> onSkillCooldownUpdate;

        [SerializeField] Skill currentSkill;

        float currentSkillCooldown;

        void Update() {
            if (currentSkillCooldown > 0f) {
                onSkillCooldownUpdate?.Invoke(currentSkillCooldown, currentSkill.cooldown);
                currentSkillCooldown -= Time.deltaTime;
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
                currentSkill.Execute(gameObject);

                currentSkillCooldown = currentSkill.cooldown;
                // TODO: call sound effect when casting skill
            } else {
                Debug.Log($"[{name}] Skill is on cooldown for {currentSkillCooldown} more seconds.");
            }
        }
    }
}