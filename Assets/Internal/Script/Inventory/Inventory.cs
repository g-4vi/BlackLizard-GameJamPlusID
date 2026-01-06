using UnityEngine;

public enum CurrencyType
{
    Mana,
    Diamond_Shard,
    Rock_Debris,
    Bat_Wing,
    Bat_Fang,
    Spider_Leg,
    Spider_Head
}

[System.Serializable]
public class Inventory
{
    public CurrencyType inventoryType;
    public int inventoryCount;
}
