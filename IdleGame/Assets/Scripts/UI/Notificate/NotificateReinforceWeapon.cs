public class NotificateReinforceWeapon : BaseNotiDot
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
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveReinforceWeaponItemNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveReinforceWeaponItemNoti -= InactiveNotiDot;
        }
    }
}
