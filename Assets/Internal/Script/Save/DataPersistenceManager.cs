using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] string fileName;
    [SerializeField] bool useEncryption;

    GameData gameData;
    List<IDataPersistence> dataPersistenceObjects;
    FileDataHandler dataHandler;

    string selectedProfileId = "1";

    protected override void Awake()
    {
        base.Awake();

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        //Find all data persistence objects including the inactive objects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame()
    {
        Debug.Log("Creating new safe file...");
        gameData = new GameData();
        gameData.Initialize();

        SaveGame();
    }

    public void SaveGame()
    {
        if(gameData == null)//no data found
        {
            Debug.LogWarning("No data found. Start the game to create new data.");
            return;
        }

        //Save all scripts with persistence data
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        dataHandler.Save(gameData, selectedProfileId);
    }

    public void LoadGame()
    {
        //Load saved data
        gameData = dataHandler.Load(selectedProfileId);

        if(gameData == null)//no save file found
        {
            NewGame();
        }

        //Push loaded data to all scripts that need the data
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

}
