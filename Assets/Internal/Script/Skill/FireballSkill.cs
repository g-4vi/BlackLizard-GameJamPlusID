using GameJamPlus.SkillModules.Behaviour;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Fireball Skill", menuName = "Skill Modules/Fireball")]
    public class FirebalSkill : Common.Skill {

        [Header("Fireball Settings")]
        [SerializeField] GameObject prefab;
        [SerializeField] float projectileSpeed = 5f;

        protected override void Execute(GameObject user) {
            // Instantiate
            GameObject fireball = Instantiate(prefab, user.transform.position, Quaternion.identity);
            if (fireball == null) {
                Debug.LogWarning($"[{name}] Failed to instantiate fireball prefab.");
                return;
            }

            // Get ProjectileBehaviour and set direction & speed
            ProjectileBehaviour projectile = fireball.GetComponent<ProjectileBehaviour>();
            Vector2 targetDir = Vector2.right * Mathf.Sign(user.transform.localScale.x);
            projectile.SetDirection(targetDir);
            projectile.SetSpeed(projectileSpeed);
        }

#if UNITY_EDITOR
        void OnValidate() {
            if (prefab != null && prefab.GetComponent<ProjectileBehaviour>() == null) {
                Debug.LogError($"[{name}] Assigned prefab does not contain a ProjectileBehaviour component. Clearing the reference.");
                prefab = null;
            }
        }
#endif

    }
}