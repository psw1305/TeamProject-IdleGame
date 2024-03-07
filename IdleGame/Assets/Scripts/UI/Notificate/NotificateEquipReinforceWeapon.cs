public class NotificateEquipReinforceWeapon : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.CheckEquipRecommendItem();
        Manager.Notificate.ActiveReinforceWeaponItemNoti += ActiveNotiDot;
        Manager.Notificate.InactiveReinforceWeaponItemNoti += InactiveNotiDot;
        Manager.Notificate.SetReinforceWeaponNoti();
    }

    private void OnDestroy()
    {
        if (Manager.Notificate != null)
        {
            Manager.Notificate.ActiveReinforceWeaponItemNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveReinforceWeaponItemNoti -= InactiveNotiDot;
        }
    }
}
