using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventoryManager
{
    private string jsonPath = Application.dataPath + "/Scripts/Json/InvenDBTest.json";
    private string jsonText;

    private ItemDataBase _itemDataBase;
    public ItemDataBase ItemDataBase => _itemDataBase;

    public ItemData SearchItem(int itemID)
    {
        List<ItemData> pickItem = _itemDataBase.ItemDB.Where(itemData => itemData.itemID == itemID).ToList();
        return pickItem[0];
    }

    public void SaveItemDataBase()
    {
            string inventoryJson = JsonUtility.ToJson(_itemDataBase, true);
            File.WriteAllText(jsonPath, inventoryJson);
            Debug.Log("저장 완료");
    }

    public void LoadItemDataBase()
    {
        jsonText = File.ReadAllText(jsonPath);
        _itemDataBase = JsonUtility.FromJson<ItemDataBase>(jsonText);
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
        if ((equipItem.hasCount == 0 && equipItem.level == 1))
        {
            Debug.Log("조건 미충족");
            return;
        }

        // 동일 아이템 타입과 장착했는지 여부를 필터하여 해당 필터에 걸린 리스트는 false처리됩니다.
        List<ItemData> filteredEquipItem = _itemDataBase.ItemDB.Where(itemdata => itemdata.type == equipItem.type
            && itemdata.equipped == true).ToList();
        foreach (var item in filteredEquipItem)
        {
            item.equipped = false;
        }

        // 새로운 아이템 장착
        equipItem.equipped = true;

        SaveItemDataBase();

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