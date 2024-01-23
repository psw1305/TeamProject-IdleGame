using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecommendArmorItemSlotNotificate : BaseNotiDot
{
    ItemData currentItemData;
    bool isRecommend;
    protected override void Start()
    {
        base.Start();
        GetItemData();
        CheckSubscribed();
        Manager.NotificateDot.SetRecommendArmorNoti();
        Manager.NotificateDot.EquipArmorItemSubscribed += CheckSubscribed;
    }

    private void GetItemData()
    {
        currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void CheckSubscribed()
    {
        if (currentItemData == Manager.NotificateDot.CheckRecommendArmorItem() && isRecommend == false)
        {
            Manager.NotificateDot.ActiveEquipArmorItemNoti += ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipArmorItemNoti += InactiveNotiDot;
            isRecommend = true;
        }
        else if (currentItemData != Manager.NotificateDot.CheckRecommendArmorItem())
        {
            Manager.NotificateDot.ActiveEquipArmorItemNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipArmorItemNoti += InactiveNotiDot;
            isRecommend = false;
        }
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveEquipArmorItemNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipArmorItemNoti -= InactiveNotiDot;
            Manager.NotificateDot.EquipArmorItemSubscribed -= CheckSubscribed;
        }
    }
}
