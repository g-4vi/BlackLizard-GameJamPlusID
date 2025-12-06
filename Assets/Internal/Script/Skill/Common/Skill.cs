using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    public abstract class Skill : ScriptableObject {

        public System.Action<float, float> OnSkillCooldownUpdate;

        [Header("Common Properties")]
        public SfxID CastSfx;

        [Header("Skill Settings")]
        public Sprite SkillIcon;
        public string SkillName;
        public float Cooldown;
        public int ManaCost;

        // Internal cooldown timer
        public bool IsReady => _cooldownTimer <= 0f;
        float _cooldownTimer;

        /// <summary>
        /// Activates the skill's effect.
        /// Parameter <paramref name="user"/> is the GameObject that uses the skill, can be null.
        /// </summary>
        public void ActivateSpell(GameObject user) {
            if (IsReady) {
                Execute(user);
                AudioManager.Instance?.PlaySFX(CastSfx);
                _cooldownTimer = Cooldown;
            } else {
                Debug.LogWarning($"[{name}] Skill is on cooldown for {_cooldownTimer} more seconds.");
            }
        }

        /// <summary>
        /// Updates the skill's internal cooldown timer.
        /// Call from an external update loop, passing in <paramref name="deltaTime"/>.
        /// </summary>
        public void Tick(float deltaTime) {
            if (_cooldownTimer > 0f) {
                _cooldownTimer -= deltaTime;
                OnSkillCooldownUpdate?.Invoke(_cooldownTimer, Cooldown);
            }
        }

        // Function to be implemented by derived skill classes to define specific skill behavior
        protected abstract void Execute(GameObject user);

    }

    /*
        How to use:
        1. Create a new script that inherits from Skill.
        2. Implement the ActivateSpell method to define the skill's behavior.
        3. Create a ScriptableObject asset of your new skill class via the Unity Editor.
        4. Assign the skill asset to PlayerSkillController.
    */
}