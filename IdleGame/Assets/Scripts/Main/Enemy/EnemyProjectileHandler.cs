using UnityEngine;

public class EnemyProjectileHandler : ProjectileHandlerBase
{
    protected override void Start()
    {
        base.Start();
    }   

    private void FixedUpdate()
    {
        TrackingTarget(Manager.Game.Player.transform.position, Speed);        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)) || Vector2.Distance(transform.position, TargetPosition) < Mathf.Epsilon)
        {
            VFXOff();
            ReleaseObject();
        }
    }
}
