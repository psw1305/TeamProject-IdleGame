using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceWeaponTypeItemNotificate : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.NotificateDot.CheckEquipmentBtnNotiState();
        Manager.NotificateDot.ActiveReinforceWeaponItemNoti += ActiveNotiDot;
        Manager.NotificateDot.InactiveReinforceWeaponItemNoti += InactiveNotiDot;
        Manager.NotificateDot.SetReinforceWeaponNoti();
    }

    private void OnDestroy()
    {
        Manager.NotificateDot.ActiveReinforceWeaponItemNoti -= ActiveNotiDot;
        Manager.NotificateDot.InactiveReinforceWeaponItemNoti -= InactiveNotiDot;
    }
}
