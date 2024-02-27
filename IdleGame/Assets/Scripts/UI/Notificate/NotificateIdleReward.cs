using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificateIdleReward : BaseNotiDot
{
    public void SetNotiDot(bool state)
    {
       if(state)
        {
            ActiveNotiDot();
        }
        else
        {
            InactiveNotiDot();
        }
    }
}
