using System;
using System.Collections.Generic;
using GameJamPlus.SkillModules.Common;

[System.Serializable]
public class GameData {
    public PlayerResourcesData playerResourcesData;
    public PlayerSkillData playerSkillData;

    public void Initialize() {
        playerResourcesData ??= new PlayerResourcesData();
        playerResourcesData.Initialize();

        playerSkillData ??= new PlayerSkillData();
        playerSkillData.Initialize();
    }
}

[System.Serializable]
public class PlayerResourcesData {
    public Inventory manaResource;
    public List<Inventory> materialResources;

    public void Initialize() {
        manaResource = new Inventory { inventoryType = CurrencyType.Mana, inventoryCount = 0 };

        materialResources ??= new List<Inventory>();
        materialResources.Clear();

        //Adds each available materials types
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType))) {
            if (type.Equals(CurrencyType.Mana)) continue;

            materialResources.Add(new Inventory {
                inventoryType = type,
                inventoryCount = 0
            });
        }
    }
}

[System.Serializable]
public class PlayerSkillData {
    public List<SkillSlot> skillSlots;

    public void Initialize() {
        skillSlots = new List<SkillSlot>();
    }

    public SkillSlot GetSkillSlotByAsset(BaseSkill skillAsset) {
        return skillSlots.Find(slot => slot.asset == skillAsset);
    }

    public void UpdateSkillSlot(SkillSlot updatedSlot, int level) {
        // try to find existing skill slot
        SkillSlot slot = GetSkillSlotByAsset(updatedSlot.asset);
        if (slot != null) {
            slot.level = level;
        } else {
            // if not found, add new skill slot
            SkillSlot newSlot = new SkillSlot {
                asset = updatedSlot.asset,
                level = level,
                cooldownTimer = 0f
            };
            skillSlots.Add(newSlot);
        }
    }
}