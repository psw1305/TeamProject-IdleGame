using System.Collections.Generic;
using System.Linq;

public class NotificateManager
{
    #region 장비 필터 Method



    //잠금이 해제된 장비가 있는지 확인합니다.
    public List<UserItemData> CheckUnlockEquipment(List<UserItemData> itemList)
    {
        return Manager.Data.Inventory.UserItemData.Where(item => item.level > 1 || item.hasCount > 0).ToList();
    }


    public bool CheckReinforceNotiState(List<UserItemData> userItemDatas)
    {
        return userItemDatas.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0 ? true : false;
    }

    public bool CheckEquipmentWeaponBtnNotiState()
    {
        return CheckReinforceNotiState(Manager.Inventory.WeaponItemList) || !CheckRecommendItem(Manager.Inventory.WeaponItemList).equipped ? true : false;
    }

    public bool CheckEquipmentArmorBtnNotiState()
    {
        return CheckReinforceNotiState(Manager.Inventory.ArmorItemList) || !CheckRecommendItem(Manager.Inventory.ArmorItemList).equipped ? true : false;
    }

    public UserItemData CheckRecommendItem(List<UserItemData> itemList)
    {
        // BUG => InvalidOperationException: Sequence contains no elements
        // 데이터 변경 후 해당 오류 코드 발생
        // 인벤토리 테이블에 장착된 아이템이 하나도 없는 경우 or 갯수가 없을 경우 생기는 버그 확인

        var recommendItem = CheckUnlockEquipment(itemList)
            .OrderBy(item => Manager.Inventory.ItemDataDictionary[item.itemID].equipStat + item.level * Manager.Inventory.ItemDataDictionary[item.itemID].reinforceEquip)
            .ToList();

        return recommendItem.Count == 0 ? null : recommendItem.Last();
    }

    #endregion

    #region 장비 버튼 알림 관련 Method

    public delegate void EquipmentNotificate();
    public EquipmentNotificate ActiveEquipmentBtnNoti;
    public EquipmentNotificate InactiveEquipmentBtnNoti;

    public bool CheckEquipmentBtnNotiState()
    {
        var _weaponRecommendItem = CheckRecommendItem(Manager.Inventory.WeaponItemList);
        var _armorRecommendItem = CheckRecommendItem(Manager.Inventory.ArmorItemList);
        if (_weaponRecommendItem == null || _armorRecommendItem == null)
        {
            return false;
        }
        return CheckReinforceNotiState(Manager.Inventory.UserInventory.UserItemData) | !_weaponRecommendItem.equipped | !_armorRecommendItem.equipped ? true : false;
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

    public void SetReinforceWeaponNoti()
    {
        if (CheckReinforceNotiState(Manager.Inventory.WeaponItemList))
        {
            ActiveReinforceWeaponItemNoti?.Invoke();
        }
        else
        {
            InactiveReinforceWeaponItemNoti?.Invoke();
        }
    }

    public void SetReinforceArmorNoti()
    {
        if (CheckReinforceNotiState(Manager.Inventory.ArmorItemList))
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



    public RecommendEquipItemNotificate SetRecommendArmorItemNoti;

    public void SetRecommendArmorNoti()
    {
        SetRecommendArmorItemNoti?.Invoke();
    }



    public void ResetRecommendDelegateSubscribed()
    {
        SetRecommendWeaponItemNoti = null;
        SetRecommendArmorItemNoti = null;
    }

    #endregion
}