public class NotificateReinforceArmor : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.NotificateDot.CheckEquipmentBtnNotiState();
        Manager.NotificateDot.ActiveReinforceArmorItemNoti += ActiveNotiDot;
        Manager.NotificateDot.InactiveReinforceArmorItemNoti += InactiveNotiDot;
        Manager.NotificateDot.SetReinforceArmorNoti();
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.ActiveReinforceArmorItemNoti -= ActiveNotiDot;
            Manager.NotificateDot.InactiveReinforceArmorItemNoti -= InactiveNotiDot;
        }
    }
}
