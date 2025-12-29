using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : Singleton<LevelSelectionManager>
{
    public LevelDisplay InteractedLevel { get; set; }
    void Start()
    {
        PlayerManager.Instance.SpawnPlayerLimitMovement();
    }

    public void InteractStage()//Player interacts with stage
    {
        HUDStageSelectHandler.Instance.ToggleStagePanel();

        RefreshStagePanel();
    }

    void RefreshStagePanel()
    {
        StagePanelHandler.Instance.OnInteractStage = null;

        if (InteractedLevel.IsUnlocked)
        {
            // Play state
            StagePanelHandler.Instance.OnInteractStage = () =>
            {
                SceneManager.LoadScene(InteractedLevel.stageBuildIndex);
            };
        }
        else
        {
            // Locked state
            if(InteractedLevel.requiredMana.inventoryCount <= 0) //free stage
            { 
                InteractedLevel.IsUnlocked = true;
                RefreshStagePanel();
            }
            StagePanelHandler.Instance.OnInteractStage = () =>
            {
                if (!InteractedLevel.CheckRequirements()) return; //requirements unfulfilled

                Debug.Log($"Stage {InteractedLevel.stageName} Unlocked");
                UpdateRequirementInventory();
                InteractedLevel.IsUnlocked = true;

                //Re-render after state change
                RefreshStagePanel();
            };
        }

        StagePanelHandler.Instance.SetupPanel(InteractedLevel);
    }


    void UpdateRequirementInventory()
    {
        foreach (var requirement in InteractedLevel.requirements)
        {
            InteractedLevel.ownedMana -= requirement.inventoryCount;
        }
    }
}
