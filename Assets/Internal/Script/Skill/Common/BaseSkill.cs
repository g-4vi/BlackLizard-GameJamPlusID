using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    public abstract class BaseSkill : ScriptableObject {
        [Header("Info")]
        public Sprite SkillIcon;
        public string SkillName;
        [TextArea] public string SkillDescription;

        [Header("Progression")]
        public SkillProgression Progression;

        [Header("Effect")]
        public SfxID CastSfx;

        /// <summary>
        /// Activates the skill's effect.
        /// </summary>
        public void ActivateSpell(GameObject owner, SkillSlot slot) {
            if (!slot.IsReady) return;

            Execute(owner, slot);

            var data = Progression.GetLevel(slot.level);
            slot.cooldownTimer = data.cooldown;

            if (CastSfx != SfxID.None) AudioManager.Instance?.PlaySFX(CastSfx);
        }

        // Function to be implemented by derived skill classes to define specific skill behavior
        protected abstract void Execute(GameObject user, SkillSlot slot);
    }
}