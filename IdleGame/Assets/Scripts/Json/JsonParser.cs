using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonParser 
{
    private string jsonPath = Application.dataPath + "/Scripts/Json/InvenDBTest.json";
    private string jsonText;

    public ItemDataBase itemDataBase;
    public PlayerInventory playerInventory;

    public void SavePlayerData()
    {
        string inventoryJson = JsonUtility.ToJson(itemDataBase, true);

        File.WriteAllText(jsonPath, inventoryJson);
    }

    public void LoadPlayerData()
    {
        jsonText = File.ReadAllText(jsonPath);
        itemDataBase = JsonUtility.FromJson<ItemDataBase>(jsonText);
    }

    public void LoadItemDataBase()
    {
        jsonText = File.ReadAllText(jsonPath);
        itemDataBase = JsonUtility.FromJson<ItemDataBase>(jsonText);
    }
}

[System.Serializable]
public class ItemDataBase
{
    public List<SlotData> ItemDB;
}

[System.Serializable]
public class SlotData
{
    public int itemID;
    public string itemName;
    public string type;
    public string rarity;
    public int level;
    public int hasCount;
    public float equipStat;
    public float reinforceEquip;
    public float retentionEffect;
    public float reinforceEffect;
    public bool equipped;
}

[System.Serializable]
public class PlayerInventory
{
    public int itemID;
    public int level;
    public int hasCount;
    public bool equiped;
}