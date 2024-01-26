using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryManager
{
    #region ItemData Fields & Properties

    private string _itemDataBaseText;

    private ItemDataBase _itemDataBase;
    private Dictionary<string, ItemData> _itemDataDictionary = new Dictionary<string, ItemData>();
    public Dictionary<string, ItemData> ItemDataDictionary => _itemDataDictionary;

    #endregion

    #region ItemDataMethod

    public void ParseItemData()
    {
        _itemDataBaseText = Manager.Resource.GetFileText("ItemDB");
        _itemDataBase = JsonUtility.FromJson<ItemDataBase>(_itemDataBaseText);
        foreach (var itemData in _itemDataBase.ItemDB)
        {
            _itemDataDictionary.Add(itemData.itemID, itemData);
        }
    }
    #endregion

    #region InventoryData Fields & Properties

    private string _userJsonText;
    private string _userItemFile;
    private InventoryDataBase _userInventoryDB;
    public InventoryDataBase PlayerInventoryDB => _userInventoryDB;
    public List<InventorySlotData> WeaponItemList => _userInventoryDB.InventorySlotData.Where(ItemData => ItemData.itemID[0] == 'W').ToList();
    public List<InventorySlotData> ArmorItemList => _userInventoryDB.InventorySlotData.Where(ItemData => ItemData.itemID[0] == 'A').ToList();

    #endregion

    #region Inventory Data Methods

    public void Initialize(string fileName)
    {
        _userItemFile = fileName;
        _userJsonText = Manager.Resource.GetFileText(fileName);
        _userInventoryDB = JsonUtility.FromJson<InventoryDataBase>(_userJsonText);
    }

    public InventorySlotData SearchItem(string itemID)
    {
        List<InventorySlotData> pickItem = _userInventoryDB.InventorySlotData.Where(itemData => itemData.itemID == itemID).ToList();
        return pickItem[0];
    }

    public void SaveSlotsData()
    {
        string inventoryJson = JsonUtility.ToJson(_userInventoryDB, true);
        //string filePath = $"/Resources/Texts/Item/{_userItemFile}.json";

        //try
        //{
        //    StreamWriter writer = new(filePath, false);
        //    writer.Write(inventoryJson);
        //    writer.Close();

        //    Debug.Log("Text written to file: " + filePath);
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError("Error writing text to file: " + filePath + "\n" + e.Message);
        //}

        //File.WriteAllText(jsonPlayerSlotDataText, inventoryJson);
    }

    #endregion

    #region Initialize Data Methods

    public void InitItem()
    {
        ParseItemData();
        CheckToInventoryDataInit();
        Manager.Game.Player.EquipmentStatModifier();
    }

    private void CheckToInventoryDataInit()
    {
        int index = 0;
        foreach (var item in _itemDataBase.ItemDB)
        {
            if (item.itemID != _userInventoryDB.InventorySlotData[index].itemID)
            {
                _userInventoryDB.InventorySlotData.Insert(index, new InventorySlotData(item.itemID, 1, 1, false));
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