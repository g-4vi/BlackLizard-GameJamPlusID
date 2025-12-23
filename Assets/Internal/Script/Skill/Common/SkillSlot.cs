using System;
using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    [Serializable]
    public class SkillSlot {
        public BaseSkill asset;

        [Header("Runtime State")]
        public int level = 1;
        public float cooldownTimer;

        public bool IsReady => cooldownTimer <= 0f;
        public SkillProgression Progression => asset != null ? asset.Progression : null;

        /// <summary>
        /// Wraps the ActivateSpell call to include this SkillSlot instance.
        /// </summary>
        public void ActivateSpell(GameObject owner) {
            asset?.ActivateSpell(owner, this);
        }

        /// <summary>
        /// Updates the skill's internal cooldown timer.
        /// </summary>
        public void Tick(float deltaTime) {
            if (cooldownTimer > 0f)
                cooldownTimer -= deltaTime;
        }
    }
}
