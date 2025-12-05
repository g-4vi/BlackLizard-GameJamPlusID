using UnityEngine;


public class LevelDisplay : MonoBehaviour
{
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
        if(isUnlocked)
        {
            //dont display the requirements any more
            //button shouldl display play
        }
        else
        {
            //Display requirements
            //button should display Unlock with call for checkrequirement when clicked
            CheckRequirements();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //give prompt that level is interactable
            //Let player interact
            if(collision.gameObject.TryGetComponent(out Player player))
            {
                Debug.Log("Player can interact with: " + stageName);
                player.canInteract = true;
                player.TriggerInteract = () =>
                {
                    InteractStage();
                };
            }

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.canInteract = false;
                player.TriggerInteract = null;
            }
        }
    }
}
