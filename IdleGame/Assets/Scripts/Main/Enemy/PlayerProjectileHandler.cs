using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileHandler : ProjectileHandlerBase
{
    public Vector2 TargetPosition;

    private void FixedUpdate()
    {
        TrackingTarget();
    }
    protected override void TrackingTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, 0.1f);
    }
}
