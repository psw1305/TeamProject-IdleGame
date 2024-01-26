using UnityEngine;
using UnityEngine.Pool;

public class ProjectileHandlerBase : MonoBehaviour
{
    [HideInInspector]
    public long Damage;
    [HideInInspector]
    public GameObject ProjectileVFX;
    [HideInInspector]
    public DamageType DamageTypeValue = DamageType.Normal;

    public float Speed = 0.1f;

    public Vector2 TargetPosition;

    public IObjectPool<GameObject> ManagedPool { get; private set; }

    public LayerMask TargetLayerMask;

    protected virtual void Start()
    {
        if (ProjectileVFX != null)
        {
            Instantiate(ProjectileVFX, transform.position, Quaternion.identity, gameObject.transform);
        }
        Destroy(gameObject, 1f);
        //Invoke("DestroyBullet", 1.5f);
    }

    protected void TrackingTarget(Vector2 targetPosition, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
        }
    }

    public void SetManagedPool(IObjectPool<GameObject> pool)
    {
        ManagedPool = pool;
    }

    public void DestroyBullet()
    {
        ManagedPool.Release(this.gameObject);
    }
}
