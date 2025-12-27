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
    //string title, Sprite image, int requiredMana, int highscore, bool isUnlocked
    public void SetupPanel(LevelDisplay level)
    {
        stageTitle.text = level.stageName;
        stageImage.sprite = level.stagePreview;
        highscoreText.text = $"High score: {level.highScore}";

        if (level.IsUnlocked)
        {
            requirementText.text = "";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Enter";
        }
        else
        {
            requirementText.text = $"Unlock for {level.requiredMana.itemCount} <sprite=0>";
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
