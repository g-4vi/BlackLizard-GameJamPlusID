using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] MaterialData[] _materials;
    public MaterialData[] Materials => _materials;
}
