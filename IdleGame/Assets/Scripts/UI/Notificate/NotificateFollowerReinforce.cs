public class NotificateFollowerReinforce : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.ActiveReinforceFollowerNoti += ActiveNotiDot;
        Manager.Notificate.InactiveReinforceFollowerNoti += InactiveNotiDot;
        Manager.Notificate.SetReinforceFollowerNoti();
    }

    private void OnDestroy()
    {
        if (Manager.Notificate != null)
        {
            Manager.Notificate.ActiveReinforceFollowerNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveReinforceFollowerNoti -= InactiveNotiDot;
        }
    }
}