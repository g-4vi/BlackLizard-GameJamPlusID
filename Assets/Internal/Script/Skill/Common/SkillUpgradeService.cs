namespace GameJamPlus.SkillModules.Common {
    public static class SkillUpgradeService {
        /// <summary>
        /// Checks if the skill in the given slot can be leveled up based on the player's resources.
        /// </summary>
        public static bool CanLevelUp(SkillSlot slot, PlayerInventory resource) {
            if (slot.asset == null) return false;

            var progression = slot.Progression;
            if (!progression.HasNextLevel(slot.level)) return false;

            var nextLevel = progression.GetLevel(slot.level + 1);

            return resource.TrySpendResource(CurrencyType.Mana, nextLevel.upgradeCost);
        }

        /// <summary>
        /// Levels up the skill in the given slot based on the player's resources.
        /// </summary>
        public static bool LevelUp(SkillSlot slot, PlayerInventory resource) {
            if (!CanLevelUp(slot, resource)) return false;

            var nextLevel = slot.Progression.GetLevel(slot.level + 1);

            slot.level++;
            slot.cooldownTimer = 0f;

            return resource.TrySpendResource(CurrencyType.Mana, nextLevel.upgradeCost);
        }

        /// <summary>
        /// Levels down the skill in the given slot and full refunds the upgrade cost to the player's resources.
        /// </summary>
        public static bool LevelDown(SkillSlot slot, PlayerInventory resource) {
            if (slot.asset == null) return false;
            if (slot.level <= 1) return false;


            slot.level--;
            slot.cooldownTimer = 0f;

            var currentLevel = slot.Progression.GetLevel(slot.level);
            resource.ManaCollected.inventoryCount += currentLevel.upgradeCost;
            return true;
        }

        /// <summary>
        /// Resets the skill in the given slot to level 1 and fully refunds all upgrade costs to the player's resources.
        /// </summary>
        public static void ResetSkill(SkillSlot slot, PlayerInventory resource) {
            while (slot.level > 1) {
                LevelDown(slot, resource);
            }
        }
    }
}