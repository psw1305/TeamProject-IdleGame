using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHandler : ProjectileHandlerBase
{
    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        TrackingTarget(Manager.Game.Player.transform.position);
    }
}
