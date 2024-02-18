using UnityEngine;

public class NotificatePlayerState : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.CheckEquipRecommendItem();
        Manager.Notificate.ActiveEquipmentBtnNoti += ActiveNotiDot;
        Manager.Notificate.InactiveEquipmentBtnNoti += InactiveNotiDot;
        Manager.Notificate.SetPlayerStateNoti();
    }

    private void OnDestroy()
    {
        if(Manager.Notificate != null)
        {
            Manager.Notificate.ActiveEquipmentBtnNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveEquipmentBtnNoti -= InactiveNotiDot;
        }
    }
}
