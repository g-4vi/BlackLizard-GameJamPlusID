using GameJamPlus.SkillModules.Behaviour;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Fireball Skill", menuName = "Skill Modules/Fireball")]
    public class FirebalSkill : Common.Skill {

        [Header("Fireball Settings")]
        [SerializeField] GameObject prefab;
        [SerializeField] float speed = 5f;

        public override void Execute(GameObject user) {
            GameObject fireball = Instantiate(prefab, user.transform.position, Quaternion.identity);

            ProjectileBehaviour projectile = fireball.GetComponent<ProjectileBehaviour>();
            if (projectile != null) {
                Vector2 targetDir = Vector2.right * Mathf.Sign(user.transform.localScale.x);
                projectile.SetDirection(targetDir);
                projectile.SetSpeed(speed);
            }
        }

    }
}