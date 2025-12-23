namespace GameJamPlus.SkillModules.Common {
    public static class SkillUpgradeService {


        public static bool CanLevelUp(SkillSlot slot, PlayerProperties resource) {
            if (slot.asset == null) return false;

            var progression = slot.asset.Progression;
            if (!progression.HasNextLevel(slot.level)) return false;

            var nextLevel = progression.GetLevel(slot.level + 1);

            return resource.mana >= nextLevel.upgradeCost;
        }

        public static bool LevelUp(SkillSlot slot, PlayerProperties resource) {
            if (!CanLevelUp(slot, resource)) return false;

            var nextLevel = slot.asset.Progression.GetLevel(slot.level + 1);

            resource.mana -= nextLevel.upgradeCost;

            slot.level++;
            slot.cooldownTimer = 0f;

            return true;
        }
    }
}