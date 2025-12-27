using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventorySystem : Singleton<InventorySystem>
{
    [Header("Mana")]
    [SerializeField] int manaCount;
    [Header("Materials")]
    [SerializeField] List<MaterialItem> materials = new List<MaterialItem>();

    void InitMaterials()
    {
        foreach (MaterialData material in ItemDatabase.Instance.Materials)
        {

                MaterialItem newMaterial = new MaterialItem
                (
                    material,
                    0
                );
                materials.Add(newMaterial);

            
        }
    }

    private void Start()
    {
        InitMaterials();
    }

    public void AddMaterial(MaterialData material, int amount)
    {
        MaterialItem matItem = materials.FirstOrDefault(m => m.materialData == material);
        if (matItem != null)
        {
            matItem.quantity += amount;
            Debug.Log($"Added {amount} of {material.name}. New quantity: {matItem.quantity}");
        }
        else
        {
            Debug.LogWarning($"Material {material.name} not found in inventory.");
        }
    }

}
