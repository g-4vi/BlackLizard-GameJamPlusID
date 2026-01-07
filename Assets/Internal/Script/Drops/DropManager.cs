using UnityEngine;

public class DropManager : Singleton<DropManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject _itemPrefab;

    public void RandomizeDrop(MaterialData[] materials, Vector3 position)
    {
        foreach (MaterialData material in materials)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand <= material.RarityValue)
            {
                // Drop the material
                GameObject materialObj = Instantiate(_itemPrefab, position, Quaternion.identity);
                Debug.Log($"Dropped material: {material.name} at position {position}");
                if (materialObj.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite)) sprite.sprite = material.Icon;
                if (materialObj.TryGetComponent<MaterialBehaviour>(out MaterialBehaviour matBehaviour)) matBehaviour.SetMaterial(material);
                Rigidbody2D rb = materialObj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Apply random force to the dropped material
                    Vector2 force = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(1f, 3f));
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            } else
            {
                Debug.Log($"No drop for material: {material.name} (rand: {rand}, rarity: {material.RarityValue}), position: {position}");
            }
        }
    }
}
