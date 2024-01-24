using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryManager
{
    #region ItemData Fields & Properties

    private string JsonItemDataBasePath = Application.dataPath + "/Scripts/Json/Tester/ItemDB.json";
    private string JsonItemDataBaseText;

    private ItemDataBase _ItemDataBase;
    private Dictionary<string, ItemData> _itemDataDictionary = new Dictionary<string, ItemData>();
    public Dictionary<string, ItemData> ItemDataDictionary => _itemDataDictionary;

    #endregion

    #region ItemDataMethod

    public void ParseItemData()
    {
        JsonItemDataBaseText = File.ReadAllText(JsonItemDataBasePath);
        _ItemDataBase = JsonUtility.FromJson<ItemDataBase>(JsonItemDataBaseText);
        foreach (var itemData in _ItemDataBase.ItemDB)
        {
            _itemDataDictionary.Add(itemData.itemID, itemData);
        }
    }
    #endregion

    #region InventoryData Fields & Properties

    private string JsonPlayerSlotDataPath = Application.dataPath + "/Scripts/Json/Tester/HSB/InvenDB_HSB.json";
    private string JsonSlotsText;
    private InventoryDataBase _PlayerInventoryDB;
    public InventoryDataBase PlayerInventoryDB => _PlayerInventoryDB;
    public List<InventorySlotData> WeaponItemList => _PlayerInventoryDB.InventorySlotData.Where(ItemData => ItemData.itemID[0] == 'W').ToList();
    public List<InventorySlotData> ArmorItemList => _PlayerInventoryDB.InventorySlotData.Where(ItemData => ItemData.itemID[0] == 'A').ToList();

    #endregion

    #region Inventory Data Methods

    public void SetDataPath(string jsonPath)
    {
        this.JsonPlayerSlotDataPath = Application.dataPath + jsonPath;
    }

    public InventorySlotData SearchItem(string itemID)
    {
        List<InventorySlotData> pickItem = _PlayerInventoryDB.InventorySlotData.Where(itemData => itemData.itemID == itemID).ToList();
        return pickItem[0];
    }

    public void SaveSlotsData()
    {
        string inventoryJson = JsonUtility.ToJson(_PlayerInventoryDB, true);
        File.WriteAllText(JsonPlayerSlotDataPath, inventoryJson);
    }

    public void LoadSlotsData()
    {
        JsonSlotsText = File.ReadAllText(JsonPlayerSlotDataPath);
        _PlayerInventoryDB = JsonUtility.FromJson<InventoryDataBase>(JsonSlotsText);
    }

    #endregion

    #region Initialize Data Methods

    public void InitItem()
    {
        ParseItemData();
        LoadSlotsData();
        CheckToInventoryDataInit();
        Manager.Game.Player.EquipmentStatModifier();
    }

    private void CheckToInventoryDataInit()
    {
        int index = 0;
        foreach (var item in _ItemDataBase.ItemDB)
        {
            if (item.itemID != _PlayerInventoryDB.InventorySlotData[index].itemID)
            {
                _PlayerInventoryDB.InventorySlotData.Insert(index, new InventorySlotData(item.itemID, 1, 1, false));
            }
            index++;
        }
        SaveSlotsData();
    }

    #endregion

    #region ItemData Control Method

    public void ChangeEquipmentItem(InventorySlotData equipItem)
    {
        if ((equipItem.hasCount == 0 && equipItem.level == 1))
        {
            return;
        }

        if (equipItem.itemID[0] == 'W')
        {
            foreach (var item in WeaponItemList)
            {
                item.equipped = false;
            }
        }
        else if (equipItem.itemID[0] == 'A')
        {
            foreach (var item in ArmorItemList)
            {
                item.equipped = false;
            }
        }

        // 새로운 아이템 장착
        equipItem.equipped = true;

        SaveSlotsData();

        Manager.Game.Player.EquipmentStatModifier();
    }

    //일괄 강화
    public void ReinforceSelectItem(InventorySlotData itemdata)
    {
        ReinforceItem(itemdata);

        SaveSlotsData();

        Manager.Game.Player.EquipmentStatModifier();
    }

    // 선택한 아이템 강화
    public void ReinforceSelectTypeItem(List<InventorySlotData> itemList)
    {
        List<InventorySlotData> targetItemlist = itemList;
        foreach (var item in targetItemlist)
        {
            ReinforceItem(item);
        }
        SaveSlotsData();

        Manager.Game.Player.EquipmentStatModifier();
    }

    private void ReinforceItem(InventorySlotData itemdata)
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

    #endregion
}

[System.Serializable]
public class InventoryDataBase
{
    public List<InventorySlotData> InventorySlotData;
}

[System.Serializable]
public class InventorySlotData
{
    public string itemID;
    public int level;
    public int hasCount;
    public bool equipped;

    public InventorySlotData(string ItemID, int Level, int HasCount, bool Equiped)
    {
        itemID = ItemID;
        level = Level;
        hasCount = HasCount;
        equipped = Equiped;
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
    public string itemID;
    public string itemName;
    public string type;
    public string statType;
    public string rarity;
    public float equipStat;
    public float reinforceEquip;
    public float retentionEffect;
    public float reinforceEffect;
}