using GameJamPlus.SkillModules.Behaviour;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Fireball Skill", menuName = "Skill Modules/Fireball")]
    public class FirebalSkill : Common.Skill {

        [Header("Fireball Settings")]
        [SerializeField] GameObject prefab;
        [SerializeField] float speed = 5f;

        public override void Execute(GameObject caster) {
            GameObject fireball = Instantiate(prefab, caster.transform.position, Quaternion.identity);

            ProjectileBehaviour projectile = fireball.GetComponent<ProjectileBehaviour>();
            if (projectile != null) {
                Vector2 lastMovementInput = caster.GetComponent<PlayerController>().lastMovementInput;
                projectile.SetDirection(lastMovementInput);
                projectile.SetSpeed(speed);
            }
        }

    }
}