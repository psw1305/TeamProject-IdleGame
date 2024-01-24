
using UnityEngine;

public class NotificateRecommendWeaponItemSlot : BaseNotiDot
{
    InventorySlotData currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.NotificateDot.SetRecommendWeaponItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        if (Manager.NotificateDot.CheckRecommendWeaponItem().equipped)
        {
            InactiveNotiDot();
            return;
        }
         if (currentItemData == Manager.NotificateDot.CheckRecommendWeaponItem())
        {
            ActiveNotiDot();
        }
        else if (currentItemData != Manager.NotificateDot.CheckRecommendWeaponItem())
        {
            InactiveNotiDot();
        }
    }
    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.SetRecommendWeaponItemNoti -= SetRecommendItemNoti;
        }
    }
}
