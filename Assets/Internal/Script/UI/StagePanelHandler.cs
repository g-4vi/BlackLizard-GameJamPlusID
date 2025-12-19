using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StagePanelHandler : Singleton<StagePanelHandler>
{
    [SerializeField] TextMeshProUGUI stageTitle;
    [SerializeField] Image stageImage;
    [SerializeField] TextMeshProUGUI requirementText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] Button interactButton;

    public UnityAction OnInteractStage;

    private void OnEnable()
    {
        PlayerManager.Instance.SetInputActionMap("UI");
    }

    private void OnDisable()
    {
        PlayerManager.Instance.SetInputActionMap("Player");
        interactButton.onClick.RemoveAllListeners();
    }

    public void SetupPanel(string title, Sprite image, int requiredMana, int highscore, bool isUnlocked)
    {
        stageTitle.text = title;
        stageImage.sprite = image;
        highscoreText.text = $"High score: {highscore}";

        if (isUnlocked)
        {
            requirementText.text = "";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Enter";
        }
        else
        {
            requirementText.text = $"Unlock for {requiredMana} <sprite=0>";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unlock";
        }

        //Refresh button listener methods
        interactButton.onClick.RemoveAllListeners();
        interactButton.onClick.AddListener(() => OnInteractStage?.Invoke());
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
