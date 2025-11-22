using UnityEngine;
using UnityEngine.InputSystem;

namespace GameJamPlus.SkillModules.Common {
    public class PlayerSkillController : MonoBehaviour {

        [SerializeField] Skill currentSkill;

        PlayerController playerController; // Say, we have a PlayerController script that manages player input

        float currentSkillCooldown;

        void Start() {
            playerController = GetComponent<PlayerController>();
            playerController.controls.Player.Attack.performed += OnAttackPerformed;
        }
        void OnDestroy() => playerController.controls.Player.Attack.performed -= OnAttackPerformed;

        void Update() {
            if (currentSkillCooldown > 0f) {
                currentSkillCooldown -= Time.deltaTime;
            }
        }

        void OnAttackPerformed(InputAction.CallbackContext context) {
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