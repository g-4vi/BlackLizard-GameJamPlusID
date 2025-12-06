using System.Collections;
using UnityEngine;

namespace GameJamPlus.SkillModules
{
    [CreateAssetMenu(fileName = "Slow Time Skill", menuName = "Skill Modules/Slow Time")]
    public class SlowTimeSkill : Common.Skill
    {
        [Header("Slow Time Settings")]
        [SerializeField] float slowFactor = 0.5f;
        [SerializeField] float slowDuration = 3f;

        protected override void Execute(GameObject user)
        {
            if (user.TryGetComponent(out PlayerMovement pm))
            {
                pm.StartCoroutine(SlowTimeSkillCourotine(pm));
            }
            else
            {
                Debug.LogWarning($"[{name}] The user does not have a PlayerMovement component.");
            }
        }

        IEnumerator SlowTimeSkillCourotine(PlayerMovement pm)
        {
            yield return new WaitForFixedUpdate();
            Time.fixedDeltaTime = 0.02f * slowFactor;
            Time.timeScale = slowFactor;
            pm.ComputeGravityByTimeScale(slowFactor);
            pm.RescaleVelocityY(1f / slowFactor);

            yield return new WaitForSeconds(slowDuration * slowFactor);

            yield return new WaitForFixedUpdate();
            Time.fixedDeltaTime = 0.02f;
            Time.timeScale = 1f;
            pm.ComputeGravityByTimeScale(1f);
            pm.RescaleVelocityY(slowFactor);
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