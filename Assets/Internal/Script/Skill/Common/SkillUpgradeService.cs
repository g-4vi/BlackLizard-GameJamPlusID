namespace GameJamPlus.SkillModules.Common {
    public static class SkillUpgradeService {
        // TODO: check again for resource origin
        public static bool CanLevelUp(SkillSlot slot, PlayerProperties resource) {
            if (slot.asset == null) return false;

            var progression = slot.Progression;
            if (!progression.HasNextLevel(slot.level)) return false;

            var nextLevel = progression.GetLevel(slot.level + 1);

            return resource.mana >= nextLevel.upgradeCost;
        }

        public static bool LevelUp(SkillSlot slot, PlayerProperties resource) {
            if (!CanLevelUp(slot, resource)) return false;

            var nextLevel = slot.Progression.GetLevel(slot.level + 1);

            resource.mana -= nextLevel.upgradeCost;

            slot.level++;
            slot.cooldownTimer = 0f;

            return true;
        }

        public static bool LevelDown(SkillSlot slot, PlayerProperties resource) {
            if (slot.asset == null) return false;
            if (slot.level <= 1) return false;

            var currentLevel = slot.Progression.GetLevel(slot.level);
            var refundAmount = currentLevel.upgradeCost / 2;

            resource.mana += refundAmount;

            slot.level--;
            slot.cooldownTimer = 0f;

            return true;
        }

        public static void ResetSkill(SkillSlot slot, PlayerProperties resource) {
            while (slot.level > 1) {
                LevelDown(slot, resource);
            }
        }

        public static SkillLevelData GetCurrentLevel(SkillSlot slot) {
            if (slot.asset == null) return null;
            return slot.Progression.GetLevel(slot.level);
        }

        public static SkillLevelData GetNextLevel(SkillSlot slot) {
            if (slot.asset == null) return null;
            return slot.Progression.GetLevel(slot.level + 1);
        }
    }
}