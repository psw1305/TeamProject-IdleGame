using System.Linq;

public class NotificateManager
{
    #region 장비 버튼 알림 관련 Method

    public delegate void EquipmentNotificate();
    public EquipmentNotificate ActiveEquipmentBtnNoti;
    public EquipmentNotificate InactiveEquipmentBtnNoti;

    public bool CheckEquipmentBtnNotiState()
    {
        if (Manager.Inventory.UserInventory.UserItemData
            .Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1)
            .ToList().Count > 0 || !CheckRecommendWeaponItem().equipped || !CheckRecommendArmorItem().equipped)
        {
            return true;
        }
        return false;
    }

    public void SetEquipmentNoti()
    {
        if (CheckEquipmentBtnNotiState())
        {
            ActiveEquipmentBtnNoti?.Invoke();
        }
        else
        {
            InactiveEquipmentBtnNoti?.Invoke();
        }
    }

    #endregion

    #region 장비 타입 버튼 알림 관련 Method

    public delegate void EquipmentTypeNotificate();
    public EquipmentTypeNotificate ActiveWeaponEquipmentBtnNoti;
    public EquipmentTypeNotificate InactiveWeaponEquipmentBtnNoti;

    public bool CheckEquipmentWeaponBtnNotiState()
    {
        if (Manager.Inventory.WeaponItemList.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0 || !CheckRecommendWeaponItem().equipped)
        {
            return true;
        }
        return false;
    }

    public void SetWeaponEquipmentNoti()
    {
        if (CheckEquipmentWeaponBtnNotiState())
        {
            ActiveWeaponEquipmentBtnNoti?.Invoke();
        }
        else
        {
            InactiveWeaponEquipmentBtnNoti?.Invoke();
        }
    }

    public EquipmentTypeNotificate ActiveArmorEquipmentBtnNoti;
    public EquipmentTypeNotificate InactiveArmorEquipmentBtnNoti;

    public bool CheckEquipmentArmorBtnNotiState()
    {
        if (Manager.Inventory.ArmorItemList.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0 || !CheckRecommendArmorItem().equipped)
        {
            return true;
        }
        return false;
    }

    public void SetArmorEquipmentNoti()
    {
        if (CheckEquipmentArmorBtnNotiState())
        {
            ActiveArmorEquipmentBtnNoti?.Invoke();
        }
        else
        {
            InactiveArmorEquipmentBtnNoti?.Invoke();
        }
    }

    #endregion

    #region 일괄 강화 알림 관련 Method

    public delegate void TypeReinforceNotificate();
    public TypeReinforceNotificate ActiveReinforceWeaponItemNoti;
    public TypeReinforceNotificate InactiveReinforceWeaponItemNoti;
    public TypeReinforceNotificate ActiveReinforceArmorItemNoti;
    public TypeReinforceNotificate InactiveReinforceArmorItemNoti;

    public bool CheckReinforceWeaponNotiState()
    {
        if (Manager.Inventory.WeaponItemList.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetReinforceWeaponNoti()
    {
        if (CheckReinforceWeaponNotiState())
        {
            ActiveReinforceWeaponItemNoti?.Invoke();
        }
        else
        {
            InactiveReinforceWeaponItemNoti?.Invoke();
        }
    }

    public bool CheckReinforceArmorNotiState()
    {
        if (Manager.Inventory.ArmorItemList.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0)
        {
            return true;
        }
        return false;
    }

    public void SetReinforceArmorNoti()
    {
        if (CheckReinforceArmorNotiState())
        {
            ActiveReinforceArmorItemNoti?.Invoke();
        }
        else
        {
            InactiveReinforceArmorItemNoti?.Invoke();
        }
    }

    #endregion

    #region 추천 장비 알림 관련 Method

    public delegate void RecommendEquipItemNotificate();
    public RecommendEquipItemNotificate SetRecommendWeaponItemNoti;

    public void SetRecommendWeaponNoti()
    {
        SetRecommendWeaponItemNoti?.Invoke();
    }

    public UserItemData CheckRecommendWeaponItem()
    {
        // BUG => InvalidOperationException: Sequence contains no elements
        // 데이터 변경 후 해당 오류 코드 발생
        // 인벤토리 테이블에 장착된 아이템이 하나도 없는 경우 or 갯수가 없을 경우 생기는 버그 확인

        return Manager.Inventory.WeaponItemList
            .Where(item => item.level > 1 || item.hasCount > 0)
            .OrderBy(item => Manager.Inventory.ItemDataDictionary[item.itemID].equipStat + item.level * Manager.Inventory.ItemDataDictionary[item.itemID].reinforceEquip)
            .ToList()
            .Last();
    }

    public RecommendEquipItemNotificate SetRecommendArmorItemNoti;

    public void SetRecommendArmorNoti()
    {
        SetRecommendArmorItemNoti?.Invoke();
    }

    public UserItemData CheckRecommendArmorItem()
    {
        return Manager.Inventory.ArmorItemList.Where(item => item.level > 1 || item.hasCount > 0).OrderBy(item => Manager.Inventory.ItemDataDictionary[item.itemID].equipStat + item.level * Manager.Inventory.ItemDataDictionary[item.itemID].reinforceEquip).ToList().Last();
    }

    public void ResetRecommendDelegateSubscribed()
    {
        SetRecommendWeaponItemNoti=null;
        SetRecommendArmorItemNoti = null;
    }

    #endregion
}