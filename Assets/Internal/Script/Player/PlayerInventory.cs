using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Singleton<PlayerInventory>, IDataPersistence
{
    public Inventory ManaCollected { get; private set; }
    //public Dictionary<CurrencyType, Inventory> MaterialsCollected { get; private set; }
    public List<Inventory> MaterialsCollected { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        //Persist across scenes
        DontDestroyOnLoad(Instance.gameObject);

        Initialize();
    }

    void Initialize()
    {
        ManaCollected = new Inventory
        {
            inventoryType = CurrencyType.Mana,
            inventoryCount = 0
        };

        MaterialsCollected = new List<Inventory>();
    }

    public void AddResource(Inventory resourceCollected)
    {
        //Add mana resource
        if(resourceCollected.inventoryType == CurrencyType.Mana)
        {
            ManaCollected.inventoryCount += resourceCollected.inventoryCount;
        }
        else //add material resource
        {
            Inventory collMat = MaterialsCollected.Find(m => m.inventoryType == resourceCollected.inventoryType);
            
            if(collMat != null)//Same type of material already collected
            {
                collMat.inventoryCount+= resourceCollected.inventoryCount;
            }
            else
            {
                collMat = new Inventory
                {
                    inventoryType = resourceCollected.inventoryType,
                    inventoryCount = resourceCollected.inventoryCount,
                };

                MaterialsCollected.Add(collMat);
            }
        }
    }

    public bool TrySpendResource(CurrencyType currency, int amount)
    {
        Inventory inv = (currency == CurrencyType.Mana)? ManaCollected : MaterialsCollected.Find(m => m.inventoryType == currency);

        if(inv == null || inv.inventoryCount < amount) { return false; }//not enough/didnt have that resource at all

        inv.inventoryCount -= amount;

        return true;
    }

    public void LoadData(GameData gameData)
    {
        Initialize();

        ManaCollected.inventoryCount = gameData.playerResourcesData.manaResource.inventoryCount;
        
        foreach(Inventory matInv in gameData.playerResourcesData.materialResources)
        {
            MaterialsCollected.Add(new Inventory
            {
                inventoryType = matInv.inventoryType,
                inventoryCount = matInv.inventoryCount
            });
        }
    }

    public void SaveData(GameData gameData)
    {
        //Save data of Mana
        gameData.playerResourcesData.manaResource.inventoryCount = ManaCollected.inventoryCount;

        //Save data of Materials
        foreach (Inventory mat in MaterialsCollected)
        {
            Inventory savedMat = gameData.playerResourcesData.materialResources.Find(m => m.inventoryType == mat.inventoryType);

            if (savedMat != null)//Gamedata already contains the type to store that material
            {
                savedMat.inventoryCount = mat.inventoryCount;
            }
            else
            {
                gameData.playerResourcesData.materialResources.Add(new Inventory
                {
                    inventoryType = mat.inventoryType,
                    inventoryCount = mat.inventoryCount
                });
            }
        }
    }

   
}
