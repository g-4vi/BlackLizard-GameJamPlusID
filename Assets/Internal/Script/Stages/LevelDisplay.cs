using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelDisplay : MonoBehaviour
{
    [SerializeField] public int stageBuildIndex;
    public string stageName;
    public Sprite stagePreview;
    public int highScore;

    [SerializeField] public Inventory[] requirements;
    [SerializeField] public Inventory requiredMana;

    [SerializeField] public Inventory[] materialDrops;
    public bool IsUnlocked { get; set; }

    //temporary player data
    public int ownedMana = 100;

    public bool CheckRequirements()//need player data
    {
        PlayerInventory inventory = PlayerInventory.Instance;
        if (requiredMana.inventoryCount > inventory.ManaCollected.inventoryCount)
        {
            Debug.Log("Not enough Mana");
            return false;
        }

        foreach (var requirement in requirements)
        {
            if(requirement.inventoryCount > PlayerInventory.Instance.MaterialsCollected.Find
                (m => m.inventoryType == requirement.inventoryType).inventoryCount)
            {
               Debug.Log("Unfulfilled Requirements");
               return false;
            }
            
        }
        Debug.Log("All requirements fulfilled");
        return true;
    }

    void SetInteractionTrigger(bool isEntering, Player player)
    {
        if (isEntering)
        {
            HUDStageSelectHandler.Instance.ToggleInteractStageText(true);

            player.canInteract = true;

            if (player.TriggerInteract == null)
            {
                player.TriggerInteract = () =>
                {
                    LevelSelectionManager.Instance.InteractedLevel = this;
                    LevelSelectionManager.Instance.InteractStage();
                };
            }
        }
        else
        {
            HUDStageSelectHandler.Instance.ToggleInteractStageText(false);

            player.canInteract = false;
            player.TriggerInteract = null;
            LevelSelectionManager.Instance.InteractedLevel = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //give prompt that level is interactable
            //Let player interact
            if(collision.gameObject.TryGetComponent(out Player player))
            {
                Debug.Log("Player can interact with: " + stageName);

                SetInteractionTrigger(true, player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                SetInteractionTrigger(false, player);
            }
        }
    }
}
