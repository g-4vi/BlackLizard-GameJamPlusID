using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class GameData
{
    PlayerResourcesData playerResourcesData;
    public void Initialize()
    {
        playerResourcesData ??= new PlayerResourcesData();
        playerResourcesData.Initialize();
    }
}

[System.Serializable]
public class PlayerResourcesData
{
    public Inventory manaResource;
    public List<Inventory> materialResources;

    public void Initialize()
    {
        manaResource = new Inventory { inventoryType = CurrencyType.Mana, inventoryCount = 0 };

        materialResources ??= new List<Inventory>();
        materialResources.Clear();

        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            materialResources.Add(new Inventory
            {
                inventoryType = type,
                inventoryCount = 0
            });
        }
    }
}
