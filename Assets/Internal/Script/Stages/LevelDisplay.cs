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

    [SerializeField] public Requirement[] requirements;
    public bool IsUnlocked { get; set; }

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

    /*void InteractStage()//Player interacts with stage
    {
        HUDStageSelectHandler.Instance.ToggleStagePanel();

        if(!IsUnlocked)
        {
            //Display requirements
            //button should display Unlock with call for checkrequirement when clicked
            //StagePanelHandler.Instance.SetupPanel(stageName, stagePreview, requirements[0].requiredNumber, highScore,false);
            StagePanelHandler.Instance.SetupPanel(this);

            StagePanelHandler.Instance.OnInteractStage= () =>
            {
                if (CheckRequirements() && !IsUnlocked)
                {
                    //Reduce the resources
                    foreach (var requirement in requirements)
                    {
                        ownedMana -= requirement.requiredNumber;
                    }
                    
                    IsUnlocked = true;

                    //Update Panel
                    UnlockedStageInteraction();
                }
            };
        }
        else
        {
            UnlockedStageInteraction();
        }
        
    }*/

    /*public void DisplayRequirements()
    {
        //StagePanelHandler.Instance.SetupPanel(stageName, stagePreview, requirements[0].requiredNumber, highScore, false);
        StagePanelHandler.Instance.SetupPanel(this);

        StagePanelHandler.Instance.OnInteractStage = () =>
        {
            if (CheckRequirements() && !IsUnlocked)
            {
                //Reduce the resources
                foreach (var requirement in requirements)
                {
                    ownedMana -= requirement.requiredNumber;
                }

                IsUnlocked = true;

                //Update Panel
                UnlockedStageInteraction();
            }
        };
    }*/

    /*public void UnlockedStageInteraction()
    {
        //dont display the requirements any more
        //button shouldl display play

        if (!IsUnlocked) return;

        StagePanelHandler.Instance.OnInteractStage = () =>
        {
            SceneManager.LoadScene(stageBuildIndex);
        };

        StagePanelHandler.Instance.SetupPanel(this);

    }*/

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
