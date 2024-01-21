using UnityEngine;

public class EquipmentPopupNotificate : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.NotificateDot.CheckEquipmentBtnNotiState();
        Manager.NotificateDot.ActiveEquipmentBtnNoti += ActiveNotiDot;
        Manager.NotificateDot.InactiveEquipmentBtnNoti += InactiveNotiDot;
        Manager.NotificateDot.SetEquipmentNoti();
    }

    private void OnDestroy()
    {
        Manager.NotificateDot.ActiveEquipmentBtnNoti -= ActiveNotiDot;
        Manager.NotificateDot.InactiveEquipmentBtnNoti -= InactiveNotiDot;
    }
}
