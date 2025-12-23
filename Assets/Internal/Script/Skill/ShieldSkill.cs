using GameJamPlus.SkillModules.Common;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "Skill Modules/Shield")]
    public class ShieldSkill : BaseSkill {
        protected override void Execute(GameObject owner, SkillSlot slot) {
            if (owner.TryGetComponent(out Player p)) {
                p.playerProperties.UpdateShield(true);
            }
        }
    }
}