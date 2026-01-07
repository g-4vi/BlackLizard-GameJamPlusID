using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "Scriptable Objects/MaterialData")]
public class MaterialData : ScriptableObject
{
    [SerializeField] Sprite _icon;
    [Tooltip("In percentage")]
    [SerializeField] int _rarityValue;
    [SerializeField] CurrencyType _materialType;

    public Sprite Icon => _icon;
    public int RarityValue => _rarityValue;
    public CurrencyType MaterialType => _materialType;



}

