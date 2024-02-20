using System.Collections.Generic;
using System.Linq;

public class InventoryManager
{
    #region ItemData Fields & Properties

    private string _itemDataBaseText;
    private ItemContainerBlueprint _itemDataBase;
    private Dictionary<string, ItemBlueprint> _itemDataDictionary = new();

    public Dictionary<string, ItemBlueprint> ItemDataDictionary => _itemDataDictionary;

    #endregion

    #region ItemDataMethod

    public void ParseItemData()
    {
        _itemDataBase = Manager.Asset.GetBlueprint("ItemDataContainer") as ItemContainerBlueprint;
        foreach (var itemData in _itemDataBase.itemDatas)
        {
            _itemDataDictionary.Add(itemData.ItemID, itemData);
        }
    }
    #endregion

    #region InventoryData Fields & Properties

    public InventoryData UserInventory { get; private set; }
    public List<UserItemData> WeaponItemList { get; private set; }
    public List<UserItemData> ArmorItemList { get; private set; }

    #endregion

    #region Inventory Data Methods

    public void Initialize()
    {
        UserInventory = Manager.Data.Inventory;
        WeaponItemList = Manager.Data.Inventory.UserItemData.Where(ItemData => ItemData.itemID[0] == 'W').ToList();
        ArmorItemList = Manager.Data.Inventory.UserItemData.Where(ItemData => ItemData.itemID[0] == 'A').ToList();
    }

    public UserItemData SearchItem(string itemID)
    {
        List<UserItemData> pickItem = UserInventory.UserItemData.Where(itemData => itemData.itemID == itemID).ToList();
        return pickItem[0];
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
        foreach (var item in _itemDataBase.itemDatas)
        {
            if (item.ItemID != UserInventory.UserItemData[index].itemID)
            {
                UserInventory.UserItemData.Insert(index, new UserItemData(item.ItemID, 1, 0, false));
            }
            index++;
        }
    }

    #endregion

    #region ItemData Control Method

    public void ChangeEquipmentItem(UserItemData equipItem)
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

        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    //일괄 강화
    public void ReinforceSelectItem(UserItemData itemdata)
    {
        ReinforceItem(itemdata);
    }

    // 선택한 아이템 강화
    public void ReinforceSelectTypeItem(List<UserItemData> itemList)
    {
        foreach (var item in itemList)
        {
            ReinforceItem(item);
        }
    }

    private void ReinforceItem(UserItemData itemdata)
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
        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }
    #endregion
}

[System.Serializable]
public class InventoryData
{
    public List<UserItemData> UserItemData;
}

[System.Serializable]
public class UserItemData
{
    public string itemID;
    public int level;
    public int hasCount;
    public bool equipped;

    public UserItemData(string ItemID, int Level, int HasCount, bool Equiped)
    {
        itemID = ItemID;
        level = Level;
        hasCount = HasCount;
        equipped = Equiped;
    }
}