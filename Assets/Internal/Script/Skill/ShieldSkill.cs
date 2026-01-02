using GameJamPlus.SkillModules.Common;
using UnityEngine;

namespace GameJamPlus.SkillModules {
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "Skill Modules/Shield")]
    public class ShieldSkill : BaseSkill {
        public GameObject shieldSfxPrefab;

        protected override void Execute(GameObject owner, SkillSlot slot) {
            if (owner.TryGetComponent(out Player p)) {
                if (shieldSfxPrefab != null) {
                    GameObject sfx = Instantiate(shieldSfxPrefab, owner.transform.position, Quaternion.identity);
                    sfx.transform.SetParent(owner.transform);
                    p.playerProperties.UpdateShield(sfx);
                } else {
                    Debug.LogWarning("Shield SFX Prefab is not assigned.");
                }
            }
        }
    }
}