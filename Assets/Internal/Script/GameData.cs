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
        manaResource = new Inventory { currencyType = CurrencyType.Mana, itemCount = 0 };

        materialResources ??= new List<Inventory>();
        materialResources.Clear();

        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            materialResources.Add(new Inventory
            {
                currencyType = type,
                itemCount = 0
            });
        }
    }
}
