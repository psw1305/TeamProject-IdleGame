using UnityEngine;

public class PlayerProjectileHandler : ProjectileHandlerBase
{
    [SerializeField] private float _speed = 0.1f;
    
    public Vector2 TargetPosition;

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition);        
    }
}
