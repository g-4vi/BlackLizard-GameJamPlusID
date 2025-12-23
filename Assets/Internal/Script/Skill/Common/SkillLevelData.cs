using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameJamPlus.SkillModules.Common {
    [Serializable]
    public class SkillLevelData {
        public int level = 1;

        [Header("Attributes")]
        public float cooldown = 20f;
        public float duration = 3f;
        public int manaCost = 0;

        [Header("Upgrade")]
        public int upgradeCost = 10;
    }

    [Serializable]
    public class SkillProgression {
        public List<SkillLevelData> levels = new List<SkillLevelData>() {
            new SkillLevelData()
        };

        public int MaxLevel => levels.Count;

        public SkillLevelData GetLevel(int level)
            => levels[level - 1];

        public bool HasNextLevel(int currentLevel)
            => currentLevel < MaxLevel;
    }
}