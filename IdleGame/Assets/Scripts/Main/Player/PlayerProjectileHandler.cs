using UnityEngine;

public class PlayerProjectileHandler : ProjectileHandlerBase
{
    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition, Speed);
    }
}
