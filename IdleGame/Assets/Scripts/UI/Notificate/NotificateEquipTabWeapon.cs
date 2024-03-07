public class NotificateEquipTabWeapon : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.ActiveWeaponEquipmentBtnNoti += ActiveNotiDot;
        Manager.Notificate.InactiveWeaponEquipmentBtnNoti += InactiveNotiDot;
        Manager.Notificate.SetWeaponEquipmentNoti();
    }

    private void OnDestroy()
    {
        if (Manager.Notificate != null)
        {
            Manager.Notificate.ActiveWeaponEquipmentBtnNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveWeaponEquipmentBtnNoti -= InactiveNotiDot;
        }
    }
}
