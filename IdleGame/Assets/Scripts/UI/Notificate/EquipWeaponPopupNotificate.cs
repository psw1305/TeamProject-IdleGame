public class EquipWeaponPopupNotificate : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.NotificateDot.ActiveWeaponEquipmentBtnNoti += ActiveNotiDot;
        Manager.NotificateDot.InactiveWeaponEquipmentBtnNoti += InactiveNotiDot;
        Manager.NotificateDot.SetWeaponEquipmentNoti();
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveWeaponEquipmentBtnNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveWeaponEquipmentBtnNoti -= InactiveNotiDot;
        }
    }
}
