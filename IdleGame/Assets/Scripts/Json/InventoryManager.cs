using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryManager
{
    private string jsonPath = Application.dataPath + "/Scripts/Json/InvenDBTest.json";
    private string jsonText;

    public ItemDataBase itemDataBase;

    public void SaveItemDataBase()
    {
            string inventoryJson = JsonUtility.ToJson(itemDataBase, true);
            File.WriteAllText(jsonPath, inventoryJson);
            Debug.Log("저장 완료");
    }

    public void LoadItemDataBase()
    {
        jsonText = File.ReadAllText(jsonPath);
        itemDataBase = JsonUtility.FromJson<ItemDataBase>(jsonText);
        Debug.Log("불러오기 완료");
    }

    public void InitItem()
    {
        LoadItemDataBase();
        Manager.Game.Player.EquipmentStatModifier();
    }

    public void ChangeItem(ItemData equipItem)
    {
        Debug.Log(equipItem.itemName);
        if ((equipItem.hasCount < 1 && equipItem.level == 1))
        {
            Debug.Log("조건 미충족");
            return;
        }

        // 동일 아이템 타입과 장착했는지 여부를 필터하여 해당 필터에 걸린 리스트는 false처리됩니다.
        List<ItemData> filteredEquipItem = itemDataBase.ItemDB.Where(itemdata => itemdata.type == equipItem.type
            && itemdata.equipped == true).ToList();
        foreach (var item in filteredEquipItem)
        {
            item.equipped = false;
        }

        // 새로운 아이템 장착
        List<ItemData> SelectItem = itemDataBase.ItemDB.Where(itemdata => itemdata.itemID == equipItem.itemID).ToList();
        SelectItem[0].equipped = true;

        SaveItemDataBase();
        LoadItemDataBase();
        Manager.Game.Player.EquipmentStatModifier();
    }
}

[System.Serializable]
public class ItemDataBase
{
    public List<ItemData> ItemDB;
}

[System.Serializable]
public class ItemData
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