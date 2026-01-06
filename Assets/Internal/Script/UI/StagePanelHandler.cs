using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StagePanelHandler : Singleton<StagePanelHandler>
{
    [SerializeField] TextMeshProUGUI stageTitle;
    [SerializeField] Image stageImage;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] Button interactButton;

    [SerializeField] GameObject detailsHolder;
    [SerializeField] TextMeshProUGUI detailsText;

    public UnityAction OnInteractStage;

    List<GameObject> detailItems = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        foreach(Transform detailTransform in detailsHolder.transform)
        {
            GameObject detailObject = detailTransform.gameObject;
            detailObject.SetActive(false);
            detailItems.Add(detailObject);
        }
    }

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
            detailsText.text = "Obtainables: ";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Enter";
        }
        else
        {
            detailsText.text = "Requirements: ";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Unlock for {level.requiredMana.inventoryCount} <sprite=0>";

            UpdateDetailContainers();

        }

        //Refresh button listener methods
        interactButton.onClick.RemoveAllListeners();
        interactButton.onClick.AddListener(() => OnInteractStage?.Invoke());
    }

    void UpdateDetailContainers()
    {
        LevelDisplay selectedLevel = LevelSelectionManager.Instance.InteractedLevel;

        if(!selectedLevel.IsUnlocked)//level locked
        {
            if (selectedLevel.requirements.Length > 0)//has requirement(s)
            {
                for(int i = 0; i < selectedLevel.requirements.Length; i++)
                {
                    Image detailImage = detailItems[i].GetComponentInChildren<Image>();
                    TextMeshProUGUI detailAmount = detailItems[i].GetComponentInChildren<TextMeshProUGUI>();
                    
                    detailImage.sprite = InventoryDatabase.GetData(selectedLevel.requirements[i].inventoryType).inventorySprite;
                    detailAmount.text = $"x{selectedLevel.requirements[i].inventoryCount.ToString()}";

                    detailItems[i].SetActive(true);
                }
            }
        }
        
    }

    public void ClosePanel()
    {
        foreach(Transform detailTransform in detailsHolder.transform)
        {
            detailTransform.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
