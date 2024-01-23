using UnityEngine;

public class RecommendWeaponItemSlotNotificate : BaseNotiDot
{
    ItemData currentItemData;
    bool isRecommend;
    protected override void Start()
    {
        base.Start();
        GetItemData();
        CheckSubscribed();
        Manager.NotificateDot.SetRecommendWeaponNoti();
        Manager.NotificateDot.EquipWeaponItemSubscribed += CheckSubscribed;
    }

    private void GetItemData()
    {
        currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void CheckSubscribed()
    {
        if (currentItemData == Manager.NotificateDot.CheckRecommendWeaponItem() && isRecommend == false)
        {
            Manager.NotificateDot.ActiveEquipWeaponItemNoti += ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipWeaponItemNoti += InactiveNotiDot;
            isRecommend = true;
        }
        else if(currentItemData != Manager.NotificateDot.CheckRecommendWeaponItem())
        {
            Manager.NotificateDot.ActiveEquipWeaponItemNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipWeaponItemNoti += InactiveNotiDot;
            isRecommend = false;
        }
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveEquipWeaponItemNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipWeaponItemNoti -= InactiveNotiDot;
            Manager.NotificateDot.EquipWeaponItemSubscribed -= CheckSubscribed;
        }
    }
}
