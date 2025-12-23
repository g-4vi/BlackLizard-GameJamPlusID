namespace GameJamPlus.SkillModules.Common {
    public static class SkillUpgradeService {
        // TODO: check again for resource origin

        /// <summary>
        /// Checks if the skill in the given slot can be leveled up based on the player's resources.
        /// </summary>
        public static bool CanLevelUp(SkillSlot slot, PlayerProperties resource) {
            if (slot.asset == null) return false;

            var progression = slot.Progression;
            if (!progression.HasNextLevel(slot.level)) return false;

            var nextLevel = progression.GetLevel(slot.level + 1);

            return resource.mana >= nextLevel.upgradeCost;
        }

        /// <summary>
        /// Levels up the skill in the given slot based on the player's resources.
        /// </summary>
        public static bool LevelUp(SkillSlot slot, PlayerProperties resource) {
            if (!CanLevelUp(slot, resource)) return false;

            var nextLevel = slot.Progression.GetLevel(slot.level + 1);

            resource.mana -= nextLevel.upgradeCost;

            slot.level++;
            slot.cooldownTimer = 0f;

            return true;
        }

        /// <summary>
        /// Levels down the skill in the given slot and full refunds the upgrade cost to the player's resources.
        /// </summary>
        public static bool LevelDown(SkillSlot slot, PlayerProperties resource) {
            if (slot.asset == null) return false;
            if (slot.level <= 1) return false;

            var currentLevel = slot.Progression.GetLevel(slot.level);
            resource.mana += currentLevel.upgradeCost;

            slot.level--;
            slot.cooldownTimer = 0f;

            return true;
        }

        /// <summary>
        /// Resets the skill in the given slot to level 1 and fully refunds all upgrade costs to the player's resources.
        /// </summary>
        public static void ResetSkill(SkillSlot slot, PlayerProperties resource) {
            while (slot.level > 1) {
                LevelDown(slot, resource);
            }
        }
    }
}