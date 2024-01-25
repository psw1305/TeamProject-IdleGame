using UnityEngine;

public class PlayerProjectileHandler : ProjectileHandlerBase
{
    [SerializeField] private float _speed = 1.0f;

    public Vector2 TargetPosition;
    private Vector2 direction;

    private void Awake()
    {
        direction = (TargetPosition - (Vector2)transform.position);
    }
    private void FixedUpdate()
    {
        TrackingTarget();        
    }
    protected override void TrackingTarget()
    {
        transform.Translate(direction.normalized * Time.deltaTime * _speed);
        //transform.position = Vector2.MoveTowards(transform.position, TargetPosition, _speed);       
    }
}
