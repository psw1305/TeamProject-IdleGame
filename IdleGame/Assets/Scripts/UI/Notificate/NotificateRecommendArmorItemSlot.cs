using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificateRecommendArmorItemSlot : BaseNotiDot
{
    private UserItemData _currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.NotificateDot.SetRecommendArmorItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        _currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        if (Manager.NotificateDot.CheckRecommendArmorItem().equipped)
        {
            InactiveNotiDot();
            return;
        }
        if (_currentItemData == Manager.NotificateDot.CheckRecommendArmorItem())
        {
            ActiveNotiDot();
        }
        else if (_currentItemData != Manager.NotificateDot.CheckRecommendArmorItem())
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
