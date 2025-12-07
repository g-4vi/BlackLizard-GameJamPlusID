using TMPro;
using UnityEngine;

public class HUDStageSelectHandler : Singleton<HUDStageSelectHandler>
{
    [Header("Interaction")]
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] string interactStageString;

    [Header("Stage Panel")]
    [SerializeField] GameObject stagePanel;
   
    public void ToggleInteractStageText(bool isActive)
    {
        interactText.gameObject.SetActive(isActive);

        if(interactText.IsActive())
        {
            interactText.text = interactStageString;
        }
    }

    public void ToggleStagePanel()
    {
        stagePanel.gameObject.SetActive(!stagePanel.activeSelf);
    }
}
