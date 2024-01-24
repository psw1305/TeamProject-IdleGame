using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class InventoryManager
{
    private string jsonPath = Application.dataPath + "/Scripts/Json/Tester/HSB/InvenDB_HSB.json";
    private string jsonText;

    private ItemDataBase _itemDataBase;
    public ItemDataBase ItemDataBase => _itemDataBase;
    public List<ItemData> WeaponItemList => _itemDataBase.ItemDB.Where(ItemData => ItemData.type == "weapon").ToList();
    public List<ItemData> ArmorItemList => _itemDataBase.ItemDB.Where(ItemData => ItemData.type == "armor").ToList();

    public void SetDataPath(string jsonPath)
    {
        this.jsonPath = Application.dataPath + jsonPath;
    }

    public ItemData SearchItem(int itemID)
    {
        List<ItemData> pickItem = _itemDataBase.ItemDB.Where(itemData => itemData.itemID == itemID).ToList();
        return pickItem[0];
    }

    public void SaveItemDataBase()
    {
        string inventoryJson = JsonUtility.ToJson(_itemDataBase, true);
        File.WriteAllText(jsonPath, inventoryJson);
    }

    public void LoadItemDataBase()
    {
        jsonText = File.ReadAllText(jsonPath);
        _itemDataBase = JsonUtility.FromJson<ItemDataBase>(jsonText);
    }

    public void InitItem()
    {
        LoadItemDataBase();
        Manager.Game.Player.EquipmentStatModifier();
    }

    public void ChangeItem(ItemData equipItem)
    {
        if ((equipItem.hasCount == 0 && equipItem.level == 1))
        {
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

    private void ReinforceTypeItem(ItemData itemdata)
    {
        while (true)
        {
            //업그레이드를 위해 필요한 아이템 수  == 15레벨 미만 = 레벨 + 1 / 15레벨 이상 15개 고정 
            if (itemdata.level < 15)
            {
                if (itemdata.hasCount <= itemdata.level)
                {
                    break;
                }
            }
            else if (itemdata.hasCount < 15)
            {
                break;
            }

            if (itemdata.level < 15)
            {
                itemdata.hasCount -= itemdata.level + 1;
                itemdata.level += 1;
            }
            else
            {
                itemdata.hasCount -= 15;
                itemdata.level += 1;
            }
        }
    }
    
    public void ReinforceSelectItem(ItemData itemdata)
    {
        ReinforceTypeItem(itemdata);

        SaveItemDataBase();

        Manager.Game.Player.EquipmentStatModifier();
    }

    // 선택한 아이템 강화
    public void ReinforceSelectTypeItem(List<ItemData> itemList)
    {
        List<ItemData> targetItemlist = itemList;
        foreach (var item in targetItemlist)
        {
            ReinforceTypeItem(item);
        }
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
    public int level;
    public int hasCount;
    public float equipStat;
    public string itemName;
    public string type;
    public string statType;
    public string rarity;
    public float reinforceEquip;
    public float retentionEffect;
    public float reinforceEffect;
    public bool equipped;
}