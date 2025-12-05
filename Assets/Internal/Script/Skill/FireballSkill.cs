using GameJamPlus.SkillModules.Behaviour;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Fireball Skill", menuName = "Skill Modules/Fireball")]
    public class FirebalSkill : Common.Skill {

        [Header("Fireball Settings")]
        [SerializeField] GameObject prefab;
        [SerializeField] float projectileSpeed = 5f;

        public override void ActivateSpell(GameObject user) {
            GameObject fireball = Instantiate(prefab, user.transform.position, Quaternion.identity);
            if (fireball == null) {
                Debug.LogWarning($"[{name}] Failed to instantiate fireball prefab.");
                return;
            }

            if (fireball.TryGetComponent(out ProjectileBehaviour projectile)) {
                Vector2 targetDir = Vector2.right * Mathf.Sign(user.transform.localScale.x);
                projectile.SetDirection(targetDir);
                projectile.SetSpeed(projectileSpeed);
            } else {
                Debug.LogWarning($"[{name}] The prefab does not have a ProjectileBehaviour component.");
                Destroy(fireball);
            }
        }

    }
}