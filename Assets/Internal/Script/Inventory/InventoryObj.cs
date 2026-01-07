using UnityEngine;

public class InventoryObj : MonoBehaviour
{
    public Inventory inventory;
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.Instance.playerMovement.limitMovement == true) return; //in stage select

        if (collected) return;

        if(collision.CompareTag("Player"))
        {
            collected = true;
            PlayerInventory.Instance.AddResource(inventory);

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
