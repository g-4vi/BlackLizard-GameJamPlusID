using System.Collections;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Slow Time Skill", menuName = "Skill Modules/Slow Time")]
    public class SlowTimeSkill : Common.Skill {
        [Header("Slow Time Settings")]
        public float slowFactor = 0.5f;
        public float slowDuration = 3f;

        public override void ActivateSpell(GameObject user) {
            if (user.TryGetComponent(out PlayerMovement pm)) {
                // TODO: Add VFX or SFX here
                pm.StartCoroutine(SlowTimeSkillCourotine(pm));
                Debug.Log($"Slow Time Skill Activated For {slowDuration} seconds at factor {slowFactor}");
            }
        }

        IEnumerator SlowTimeSkillCourotine(PlayerMovement pm) {
            yield return new WaitForFixedUpdate();                                  // wait a frame to avoid issues with Player RigidBody when called mid-fixedupdate
            pm.ComputeGravityByTimeScale(slowFactor);
            pm.RescaleVelocityY(1f / slowFactor);

            Time.timeScale = slowFactor;
            Time.fixedDeltaTime = 0.02f * slowFactor;

            yield return new WaitForSeconds(slowDuration * slowFactor);
            yield return new WaitForFixedUpdate();                                  // same here
            pm.ComputeGravityByTimeScale(1f);
            pm.RescaleVelocityY(slowFactor);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

    }
}

/*
    Ok indo aja kali ya, ini skill slow time nya manfaatin Time.timeScale buat nge-slow nya.
    Jadi kalau misal mau ada objek yang ga ke slow pergerakannya, jangan pake Time.deltaTime, tapi pake Time.unscaledDeltaTime.
    Kecuali kalau pakai physics (RigidBody), harus handle manual itungannya.
    Beberapa script udah ku update dikit buat nyesuain skill ini.
    - Thyyn
*/