using System;
using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    [Serializable]
    public class SkillSlot {
        public BaseSkill asset;

        [Header("Runtime State")]
        public int level = 1;
        public float cooldownTimer;

        public bool IsReady => cooldownTimer <= 0f;
    }
}
