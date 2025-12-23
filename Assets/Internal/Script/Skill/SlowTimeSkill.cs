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
                pm.StartCoroutine(SlowTimeSkillCourotine(slot, pm));
            } else if (owner.TryGetComponent(out MonoBehaviour mb)) {
                mb.StartCoroutine(SlowTimeSkillCourotine(slot, null));
            }
        }

        IEnumerator SlowTimeSkillCourotine(SkillSlot slot, PlayerMovement pm) {
            var data = Progression.GetLevel(slot.level);

            yield return new WaitForFixedUpdate();
            SlowTimeEffect(pm);

            yield return new WaitForSeconds(data.duration * slowFactor);

            yield return new WaitForFixedUpdate();
            BackToNormalTime(pm);
        }

        void SlowTimeEffect(PlayerMovement pm) {
            Time.fixedDeltaTime = 0.02f * slowFactor;
            Time.timeScale = slowFactor;
            pm?.ComputeGravityByTimeScale(slowFactor);
            pm?.RescaleVelocityY(1f / slowFactor);
        }

        void BackToNormalTime(PlayerMovement pm) {
            Time.fixedDeltaTime = 0.02f;
            Time.timeScale = 1f;
            pm?.ComputeGravityByTimeScale(1f);
            pm?.RescaleVelocityY(1f);
        }

    }
}

/*
    Ok indo aja kali ya, ini skill slow time nya manfaatin Time.timeScale buat nge-slow nya.
    Jadi kalau misal mau ada objek yang ga ke slow pergerakannya, pake aja Time.unscaledDeltaTime.
    Kecuali kalau pakai physics (RigidBody), harus handle manual itungannya.
    Beberapa script udah ku update dikit buat nyesuain skill ini.
    - Thyyn
*/