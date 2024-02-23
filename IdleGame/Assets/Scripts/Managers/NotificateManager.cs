using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificateManager
{
    #region 장비 알림 필터

    //잠금이 해제된 장비가 있는지 확인합니다.
    public List<UserItemData> CheckUnlockEquipment(List<UserItemData> itemList)
    {
        return itemList.Where(item => item.level > 1 || item.hasCount > 0).ToList();
    }


    public bool CheckReinforceNotiState(List<UserItemData> userItemDatas)
    {
        //return userItemDatas.Where(item => item.hasCount >= 15 || item.hasCount >= item.level + 1).ToList().Count > 0 ? true : false;
        var notiList = userItemDatas.Where(data => data.hasCount >= Mathf.Min(data.level + 1, 15)).ToList();
        if (notiList.Count == 0)
        {
            return false;
        }
        else if (notiList.Count == 1
            & notiList[0].itemID == Manager.Data.WeaponInvenList.Last().itemID & notiList[0].level >= 100)
        {
            return false;
        }
        else if (notiList.Count == 1
            & notiList[0].itemID == Manager.Data.ArmorInvenList.Last().itemID & notiList[0].level >= 100)
        {
            return false;
        }
        else if (notiList.Count == 2
    & (notiList[0].itemID == Manager.Data.WeaponInvenList.Last().itemID & notiList[0].level >= 100 | notiList[0].itemID == Manager.Data.ArmorInvenList.Last().itemID & notiList[0].level >= 100))
        {
            return false;
        }
        else
        {
            return true;
        }
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
        var recommendItem = CheckUnlockEquipment(itemList)
            .OrderBy(item => Manager.Inventory.ItemDataDictionary[item.itemID].EquipStat + item.level * Manager.Inventory.ItemDataDictionary[item.itemID].ReinforceEquip)
            .ToList();

        return recommendItem.Count == 0 ? null : recommendItem.Last();
    }

    #endregion

    #region 메인 화면 장비 버튼

    public delegate void EquipmentNotificate();
    public event EquipmentNotificate ActiveEquipmentBtnNoti;
    public event EquipmentNotificate InactiveEquipmentBtnNoti;

    //추천 장비 착용 여부를 체크
    public bool CheckEquipRecommendItem()
    {
        var _weaponRecommendItem = CheckRecommendItem(Manager.Inventory.WeaponItemList);
        var _armorRecommendItem = CheckRecommendItem(Manager.Inventory.ArmorItemList);
        // 장비가 아예 없을 경우 false 반환
        if (_weaponRecommendItem == null | _armorRecommendItem == null)
        {
            return false;
        }
        //아니라면 추천 장비를 착용하고 있는 지
        return CheckReinforceNotiState(Manager.Inventory.UserInventory.UserItemData) | !_weaponRecommendItem.equipped | !_armorRecommendItem.equipped;
    }

    public void SetPlayerStateNoti()
    {
        if (CheckEquipRecommendItem() | CheckSkillReinforceState() | CheckFollowerReinforceState())
        {
            ActiveEquipmentBtnNoti?.Invoke();
        }
        else
        {
            InactiveEquipmentBtnNoti?.Invoke();
        }
    }

    #endregion

    #region 장비 타입 버튼 알림

    public delegate void EquipmentTypeNotificate();
    public event EquipmentTypeNotificate ActiveWeaponEquipmentBtnNoti;
    public event EquipmentTypeNotificate InactiveWeaponEquipmentBtnNoti;

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

    public event EquipmentTypeNotificate ActiveArmorEquipmentBtnNoti;
    public event EquipmentTypeNotificate InactiveArmorEquipmentBtnNoti;

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

    #region 일괄 강화 알림 관련

    public delegate void EquipReinforceNotificate();
    public event EquipReinforceNotificate ActiveReinforceWeaponItemNoti;
    public event EquipReinforceNotificate InactiveReinforceWeaponItemNoti;

    public event EquipReinforceNotificate ActiveReinforceArmorItemNoti;
    public event EquipReinforceNotificate InactiveReinforceArmorItemNoti;

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

    #region 추천 장비 알림 관련

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

    #region 스킬 강화 알림
    public delegate void SkillReinforceNotificate();
    public event SkillReinforceNotificate ActiveReinforceSkillNoti;
    public event SkillReinforceNotificate InactiveReinforceSkillNoti;

    private bool CheckSkillReinforceState()
    {
        var notiList = Manager.Data.UserSkillData.UserInvenSkill.Where(data => data.hasCount >= Mathf.Min(data.level + 1, 15)).ToList();
        if (notiList.Count == 0)
        {
            return false;
        }
        else if (notiList.Count == 1
            & notiList[0].itemID == Manager.Data.SkillInvenList.Last().itemID & notiList[0].level >= 100)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetReinforceSkillNoti()
    {
        if (CheckSkillReinforceState())
        {
            ActiveReinforceSkillNoti?.Invoke();
        }
        else
        {
            InactiveReinforceSkillNoti?.Invoke();
        }
    }

    #endregion

    #region 동료 강화 알림
    public delegate void FollowerReinforceNotificate();
    public event FollowerReinforceNotificate ActiveReinforceFollowerNoti;
    public event FollowerReinforceNotificate InactiveReinforceFollowerNoti;

    private bool CheckFollowerReinforceState()
    {
        var notiList = Manager.Data.FollowerData.UserInvenFollower.Where(data => data.hasCount >= Mathf.Min(data.level + 1, 15)).ToList();
        if (notiList.Count == 0)
        {
            return false;
        }
        else if (notiList.Count == 1
            & notiList[0].itemID == Manager.Data.FollowerInvenList.Last().itemID & notiList[0].level >= 100)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetReinforceFollowerNoti()
    {
        if (CheckFollowerReinforceState())
        {
            ActiveReinforceFollowerNoti?.Invoke();
        }
        else
        {
            InactiveReinforceFollowerNoti?.Invoke();
        }
    }

    #endregion
}