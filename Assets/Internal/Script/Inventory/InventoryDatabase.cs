using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InventoryDatabase
{
    static Dictionary<CurrencyType, InventoryData> lookup;

    public static void Initialize()
    {
        if(lookup != null) return;

        lookup = Resources.LoadAll<InventoryData>("Inventory").ToDictionary(k => k.type);
    }

    public static InventoryData GetData(CurrencyType type)
    {
        Initialize();
        return lookup[type];
    }
}
