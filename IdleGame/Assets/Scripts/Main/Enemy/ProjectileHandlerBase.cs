using UnityEngine;

public class ProjectileHandlerBase : ObjectPoolable
{
    [HideInInspector]
    public long Damage;
    [HideInInspector]
    public GameObject ProjectileVFX;
    [HideInInspector]
    public DamageType DamageTypeValue = DamageType.Normal;

    public float Speed = 0.1f;

    public Vector2 TargetPosition;

    public LayerMask TargetLayerMask;

    protected virtual void Start()
    {
        if (ProjectileVFX != null)
        {
            Instantiate(ProjectileVFX, transform.position, Quaternion.identity, gameObject.transform);
        }
    }

    protected void TrackingTarget(Vector2 targetPosition, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)) || Vector2.Distance(transform.position, TargetPosition) < Mathf.Epsilon)
        {
            //ReleaseObject();
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
        }
    }
}
