using System.Collections;
using UnityEngine;

public class ProjectileHandlerBase : MonoBehaviour
{
    [HideInInspector]
    public long Damage;
    [HideInInspector]
    public GameObject ProjectileVFX;
    [HideInInspector]
    public DamageType DamageTypeValue = DamageType.Normal;

    public LayerMask TargetLayerMask;
    protected virtual void Start()
    {
        if (ProjectileVFX != null)
        {
            Instantiate(ProjectileVFX, transform.position, Quaternion.identity, gameObject.transform);
        }
        StartCoroutine(ProjectileLifeCycle());
    }

    protected virtual void TrackingTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, Manager.Game.Player.transform.position, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
            Destroy(gameObject);
        }
    }

    IEnumerator ProjectileLifeCycle()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
