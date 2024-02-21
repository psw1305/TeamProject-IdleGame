using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerProjectileHandler : ProjectileHandlerBase
{
    protected override void Start()
    {
        base.Start();
    }
    
    private void FixedUpdate()
    {
        TrackingTarget(Manager.Game.Player.transform.position, Speed);
        if (Vector2.Distance(transform.position, Manager.Game.Player.transform.position) < Mathf.Epsilon)
        {
            ReleaseObject();
        }
    }
}
