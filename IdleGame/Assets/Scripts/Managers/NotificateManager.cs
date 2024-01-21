using System.Linq;

public class NotificateManager
{

    #region EquipmentBtn Method

    public delegate void EquipmentNotificate();
    public EquipmentNotificate ActiveEquipmentBtnNoti;
    public EquipmentNotificate InactiveEquipmentBtnNoti;

    public bool CheckEquipmentBtnNotiState()
    {
        if (Manager.Inventory.ItemDataBase.ItemDB.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0)
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

    public delegate void RecommendEquipItemNotificate();
    public RecommendEquipItemNotificate ActiveEquipWeaponItemNoti;
    public RecommendEquipItemNotificate InactiveEquipWeaponItemNoti;
    public RecommendEquipItemNotificate ActiveEquipArmorItemNoti;
    public RecommendEquipItemNotificate InactiveEquipArmorItemNoti;

    public void SetRecommendWeaponNoti(ItemData itemData)
    {
        if (itemData != notifyBestStatItem())
        {
            return;
        }
        if (notifyBestStatItem().equipped)
        {
            ActiveReinforceWeaponItemNoti?.Invoke();
        }
        else
        {
            InactiveReinforceWeaponItemNoti?.Invoke();
        }
    }

    public ItemData notifyBestStatItem()
    {
        return Manager.Inventory.WeaponItemList.OrderBy(item => item.equipStat + item.level * item.reinforceEquip).ToList()[0];
    }
}