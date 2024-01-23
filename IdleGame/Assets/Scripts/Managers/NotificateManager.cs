using System.Linq;
using UnityEngine;

public class NotificateManager
{

    #region 장비 버튼 알림 관련 Method

    public delegate void EquipmentNotificate();
    public EquipmentNotificate ActiveEquipmentBtnNoti;
    public EquipmentNotificate InactiveEquipmentBtnNoti;

    public bool CheckEquipmentBtnNotiState()
    {
        if (Manager.Inventory.ItemDataBase.ItemDB.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0 || !CheckRecommendWeaponItem().equipped || !CheckRecommendArmorItem().equipped)
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

    #region

    public delegate void RecommendEquipItemNotificate();
    public RecommendEquipItemNotificate ActiveEquipWeaponItemNoti;
    public RecommendEquipItemNotificate InactiveEquipWeaponItemNoti;
    public RecommendEquipItemNotificate EquipWeaponItemSubscribed;

    public void SetRecommendWeaponNoti()
    {
        if (!CheckRecommendWeaponItem().equipped)
        {
            InactiveEquipWeaponItemNoti?.Invoke();
            ActiveEquipWeaponItemNoti?.Invoke();
        }
        else
        {
            InactiveEquipWeaponItemNoti?.Invoke();
        }
    }

    public void SetEquipWeaponItemSubscribed()
    {
        EquipWeaponItemSubscribed?.Invoke();
    }

    public ItemData CheckRecommendWeaponItem()
    {
        return Manager.Inventory.WeaponItemList.Where(item => item.level > 1 || item.hasCount > 0).OrderBy(item => item.equipStat + item.level * item.reinforceEquip).ToList().Last();
    }

    public RecommendEquipItemNotificate ActiveEquipArmorItemNoti;
    public RecommendEquipItemNotificate InactiveEquipArmorItemNoti;
    public RecommendEquipItemNotificate EquipArmorItemSubscribed;

    public void SetRecommendArmorNoti()
    {
        if (!CheckRecommendArmorItem().equipped)
        {
            InactiveEquipArmorItemNoti?.Invoke();
            ActiveEquipArmorItemNoti?.Invoke();
        }
        else
        {
            InactiveEquipArmorItemNoti?.Invoke();
        }
    }

    public void SetEquipArmorItemSubscribed()
    {
        EquipArmorItemSubscribed?.Invoke();
    }

    public ItemData CheckRecommendArmorItem()
    {
        return Manager.Inventory.ArmorItemList.Where(item => item.level > 1 || item.hasCount > 0).OrderBy(item => item.equipStat + item.level * item.reinforceEquip).ToList().Last();
    }

    #endregion
}