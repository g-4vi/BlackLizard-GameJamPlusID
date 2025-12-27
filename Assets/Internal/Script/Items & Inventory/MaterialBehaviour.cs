using UnityEngine;

public class MaterialBehaviour : MonoBehaviour
{
    [SerializeField] MaterialData _material;

    public void SetMaterial(MaterialData mat)
    {
        _material = mat;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(
        $"Hit name: {collision.gameObject.name}, " +
        $"Tag: [{collision.gameObject.tag}]"
    );
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player collected material: {gameObject.name}");
            InventorySystem.Instance.AddMaterial(_material, 1);
            
            
            Destroy(gameObject);
        }
    }
}
