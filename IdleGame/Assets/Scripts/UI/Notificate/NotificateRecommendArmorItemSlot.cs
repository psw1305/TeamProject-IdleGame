using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificateRecommendArmorItemSlot : BaseNotiDot
{
    UserItemData currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.NotificateDot.SetRecommendArmorItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        if (Manager.NotificateDot.CheckRecommendArmorItem().equipped)
        {
            InactiveNotiDot();
            return;
        }
        if (currentItemData == Manager.NotificateDot.CheckRecommendArmorItem())
        {
            ActiveNotiDot();
        }
        else if (currentItemData != Manager.NotificateDot.CheckRecommendArmorItem())
        {
            InactiveNotiDot();
        }
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.SetRecommendArmorItemNoti -= SetRecommendItemNoti;
        }
    }
}
