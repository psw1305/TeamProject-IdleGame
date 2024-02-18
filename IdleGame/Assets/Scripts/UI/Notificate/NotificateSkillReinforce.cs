public class NotificateSkillReinforce : BaseNotiDot
{
    protected override void Start()
    {
        base.Start();
        Manager.Notificate.ActiveReinforceSkillNoti += ActiveNotiDot;
        Manager.Notificate.InactiveReinforceSkillNoti += InactiveNotiDot;
        Manager.Notificate.SetReinforceSkillNoti();
    }

    private void OnDestroy()
    {
        if(Manager.Notificate != null)
        {
            Manager.Notificate.ActiveReinforceSkillNoti -= ActiveNotiDot;
            Manager.Notificate.InactiveReinforceSkillNoti -= InactiveNotiDot;
        }
    }
}
