using UnityEngine;

public class ProjectileHandlerBase : ObjectPoolable
{
    [HideInInspector]
    public long Damage;
    public GameObject ProjectileVFX;
    [HideInInspector]
    public DamageType DamageTypeValue = DamageType.Normal;

    public float Speed = 0.1f;

    public Vector2 TargetPosition;

    public LayerMask TargetLayerMask;

    public SpriteRenderer projectileSprite;

    private bool has;

    protected virtual void Start()
    {
        if (ProjectileVFX != null)
        {
            GameObject go = Instantiate(ProjectileVFX, transform.position, Quaternion.identity, gameObject.transform);
            int index = go.name.IndexOf("(Clone)");
            if (index > 0)
            {
                go.name = go.name.Substring(0, index);
            }
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
            ReleaseObject();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
        }
    }

    public void SetProjectile(GameObject VFX, long Damage)
    {
        this.Damage = Damage;
        this.ProjectileVFX = VFX;

        has = false;
        if (ProjectileVFX != null && transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.name == ProjectileVFX.name)
                {
                    has = true;
                    this.transform.GetChild(i).gameObject.SetActive(true);
                    break;
                }
            }
            if (!has)
            {
                GameObject go = Instantiate(ProjectileVFX, transform.position, Quaternion.identity, gameObject.transform);
                int index = go.name.IndexOf("(Clone)");
                if (index > 0)
                {
                    go.name = go.name.Substring(0, index);
                }
            }
        }
    }
}
