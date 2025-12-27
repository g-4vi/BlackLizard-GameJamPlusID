using UnityEngine;

public enum CurrencyType
{
    Mana
}

[System.Serializable]
public class Inventory
{
    public CurrencyType currencyType;
    public int itemCount;
   
}
