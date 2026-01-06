using System.Collections.Generic;
using UnityEngine;

//for handle resource collection during gameplay
public class PlayerCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.Instance.playerMovement.limitMovement == true) return; //in stage select

        if(collision.CompareTag("Resource"))
        {
            if(collision.TryGetComponent(out InventoryObj resourceObj))
            {
                 PlayerInventory.Instance.AddResource(resourceObj.inventory);

                if(collision.gameObject != null)
                {
                    Destroy(collision.gameObject);
                }
            }
            
        }
    }
}
