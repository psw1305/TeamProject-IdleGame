using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceArmorTypeItemNotificate : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.NotificateDot.CheckEquipmentBtnNotiState();
        Manager.NotificateDot.ActiveReinforceArmorItemNoti += ActiveNotiDot;
        Manager.NotificateDot.InactiveReinforceArmorItemNoti += InactiveNotiDot;
        Manager.NotificateDot.SetReinforceArmorNoti();
    }

    private void OnDestroy()
    {
        Manager.NotificateDot.ActiveReinforceArmorItemNoti -= ActiveNotiDot;
        Manager.NotificateDot.InactiveReinforceArmorItemNoti -= InactiveNotiDot;
    }
}
