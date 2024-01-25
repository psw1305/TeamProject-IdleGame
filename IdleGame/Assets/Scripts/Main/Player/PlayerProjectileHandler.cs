using UnityEngine;

public class PlayerProjectileHandler : ProjectileHandlerBase
{
    [SerializeField] private float _speed = 0.1f;
    
    public Vector2 TargetPosition;
    private Vector2 direction;

    protected override void Start()
    {
        base.Start();
        direction = (TargetPosition - (Vector2)transform.position);
    }

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition);        
    }

    //protected override void TrackingTarget()
    //{
    //    //transform.Translate(direction.normalized * Time.deltaTime * _speed);
    //    transform.position = Vector2.MoveTowards(transform.position, TargetPosition, _speed);       
    //    TrackingTarget(TargetPosition);
    //}
}
