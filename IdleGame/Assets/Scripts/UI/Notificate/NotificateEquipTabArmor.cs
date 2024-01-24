using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificateEquipTabArmor : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.NotificateDot.ActiveArmorEquipmentBtnNoti += ActiveNotiDot;
        Manager.NotificateDot.InactiveArmorEquipmentBtnNoti += InactiveNotiDot;
        Manager.NotificateDot.SetArmorEquipmentNoti();
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveArmorEquipmentBtnNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveArmorEquipmentBtnNoti -= InactiveNotiDot;
        }
    }
}
