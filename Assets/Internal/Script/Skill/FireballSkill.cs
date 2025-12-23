using GameJamPlus.SkillModules.Behaviour;
using GameJamPlus.SkillModules.Common;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Fireball Skill", menuName = "Skill Modules/Fireball")]
    public class FirebalSkill : BaseSkill {

        [Header("Fireball Settings")]
        [SerializeField] GameObject prefab;
        [SerializeField] float projectileSpeed = 5f;

        protected override void Execute(GameObject owner, SkillSlot slot) {
            // Instantiate
            GameObject fireball = Instantiate(prefab, owner.transform.position, Quaternion.identity);

            // Get ProjectileBehaviour and set direction & speed
            ProjectileBehaviour projectile = fireball.GetComponent<ProjectileBehaviour>();
            Vector2 targetDir = Vector2.right * Mathf.Sign(owner.transform.localScale.x);
            projectile.SetDirection(targetDir);
            projectile.SetSpeed(projectileSpeed);
        }

#if UNITY_EDITOR
        // Validation to ensure prefab has ProjectileBehaviour component
        void OnValidate() {
            if (prefab != null && prefab.GetComponent<ProjectileBehaviour>() == null) {
                Debug.LogError($"[{name}] Assigned prefab does not contain a ProjectileBehaviour component. Clearing the reference.");
                prefab = null;
            }
        }
#endif

    }
}