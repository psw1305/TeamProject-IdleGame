using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificateEquipTabArmor : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.ActiveArmorEquipmentBtnNoti += ActiveNotiDot;
        Manager.Notificate.InactiveArmorEquipmentBtnNoti += InactiveNotiDot;
        Manager.Notificate.SetArmorEquipmentNoti();
    }

    private void OnDestroy()
    {
        if (Manager.Notificate != null)
        {
            Manager.Notificate.ActiveArmorEquipmentBtnNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveArmorEquipmentBtnNoti -= InactiveNotiDot;
        }
    }
}
