using UnityEngine;

public enum CurrencyType
{
    Mana
}

[System.Serializable]
public class Requirement
{
    public CurrencyType currencyType;
    public int requiredNumber;
}
