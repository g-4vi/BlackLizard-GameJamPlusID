using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "Skill Modules/Shield")]
    public class ShieldSkill : Common.Skill {
        protected override void Execute(GameObject user) {
            if (user.TryGetComponent(out Player p)) {
                p.playerProperties.UpdateShield(true);
            } else {
                Debug.LogWarning($"[{name}] The user does not have a Player component.");
            }
        }
    }
}