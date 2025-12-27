using System.Collections;
using GameJamPlus.SkillModules.Common;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Slow Time Skill", menuName = "Skill Modules/Slow Time")]
    public class SlowTimeSkill : BaseSkill {
        [Header("Slow Time Settings")]
        [SerializeField] float slowFactor = 0.5f;

        protected override void Execute(GameObject owner, SkillSlot slot) {
            if (owner.TryGetComponent(out PlayerMovement pm)) {
                // Player object
                pm.StartCoroutine(SlowTimeSkillCourotine(slot, pm));
            } else if (owner.TryGetComponent(out MonoBehaviour mb)) {
                // Fallback for non-player objects
                mb.StartCoroutine(SlowTimeSkillCourotine(slot, null));
            }
        }

        IEnumerator SlowTimeSkillCourotine(SkillSlot slot, PlayerMovement pm) {
            var data = Progression.GetLevel(slot.level);

            // Start slowing time
            yield return new WaitForFixedUpdate();
            pm.RescaleVelocityY(1f / slowFactor);
            SlowTime(slowFactor, pm);

            // Wait for duration
            yield return new WaitForSeconds(data.duration * slowFactor);

            // Restore time
            yield return new WaitForFixedUpdate();
            pm.RescaleVelocityY(slowFactor);
            SlowTime(1f, pm);
        }

        void SlowTime(float targetSlow, PlayerMovement pm = null) {
            Time.fixedDeltaTime = 0.02f * targetSlow;
            Time.timeScale = targetSlow;
            pm?.ComputeGravityByTimeScale(Time.timeScale);
        }

    }
}