using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "Skill Modules/Shield")]
    public class ShieldSkill : Common.Skill {
        public override void ActivateSpell(GameObject user) {
            if (user.TryGetComponent(out Player p)) {
                p.playerProperties.UpdateShield(true);
                Debug.Log($"[{name}] Shield activated for player.");
            } else {
                Debug.LogWarning($"[{name}] The user does not have a PlayerProperties component.");
            }
        }
    }
}