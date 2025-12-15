using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelDisplay : MonoBehaviour
{
    [SerializeField] int stageBuildIndex;
    public string stageName;
    public Sprite stagePreview;
    public int highScore;

    [SerializeField] Requirement[] requirements;
    bool isUnlocked;

    //temporary player data
    public int ownedMana = 100;

    public bool CheckRequirements()//need player data
    {
        foreach (var requirement in requirements)
        {
            if(requirement.requiredNumber >= ownedMana)
            {
               Debug.Log("Unfulfilled Requirements");
               return false;
            }
            
        }
        Debug.Log("All requirements fulfilled");
        return true;
    }

    void InteractStage()//Player interacts with stage
    {
        HUDStageSelectHandler.Instance.ToggleStagePanel();

        if(!isUnlocked)
        {
            //Display requirements
            //button should display Unlock with call for checkrequirement when clicked
            StagePanelHandler.Instance.SetupPanel(stageName, stagePreview, requirements[0].requiredNumber, highScore,false);

            StagePanelHandler.Instance.OnInteractStage= () =>
            {
                if (CheckRequirements() && !isUnlocked)
                {
                    //Reduce the resources
                    foreach (var requirement in requirements)
                    {
                        ownedMana -= requirement.requiredNumber;
                    }
                    
                    isUnlocked = true;

                    //Update Panel
                    UnlockedStageInteraction();
                }
            };
        }
        else
        {
            UnlockedStageInteraction();
        }
        
    }

    void UnlockedStageInteraction()
    {
        //dont display the requirements any more
        //button shouldl display play

        if (!isUnlocked) return;

        StagePanelHandler.Instance.OnInteractStage = () =>
        {
            SceneManager.LoadScene(stageBuildIndex);
        };

        StagePanelHandler.Instance.SetupPanel(stageName, stagePreview, requirements[0].requiredNumber, highScore, true);

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

                HUDStageSelectHandler.Instance.ToggleInteractStageText(true);

                player.canInteract = true;

                if(player.TriggerInteract == null)
                {
                    player.TriggerInteract = () =>
                    {
                        InteractStage();
                    };
                }
            }

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                HUDStageSelectHandler.Instance.ToggleInteractStageText(false);
                
                player.canInteract = false;
                player.TriggerInteract = null;
            }
        }
    }
}
