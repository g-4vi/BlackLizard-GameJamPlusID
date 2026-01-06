using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory Data", fileName = "New_Inventory")]
public class InventoryData : ScriptableObject
{
    public CurrencyType type;
    public string inventoryName;
    public Sprite inventorySprite;
}
