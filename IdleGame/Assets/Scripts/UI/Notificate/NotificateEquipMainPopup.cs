using UnityEngine;

public class NotificateEquipMainPopup : BaseNotiDot
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
        if(Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveEquipmentBtnNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveEquipmentBtnNoti -= InactiveNotiDot;
        }
    }
}
