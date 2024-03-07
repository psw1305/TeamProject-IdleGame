public class NotificateEquipReinforceArmor : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.CheckEquipRecommendItem();
        Manager.Notificate.ActiveReinforceArmorItemNoti += ActiveNotiDot;
        Manager.Notificate.InactiveReinforceArmorItemNoti += InactiveNotiDot;
        Manager.Notificate.SetReinforceArmorNoti();
    }

    private void OnDestroy()
    {
        if (Manager.Notificate != null)
        {
            Manager.Notificate.ActiveReinforceArmorItemNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveReinforceArmorItemNoti -= InactiveNotiDot;
        }
    }
}
