using UnityEngine;

public class MeteorProjectile : SkillProjectileHandlerBase
{
    [SerializeField] private GameObject destroyVFX;

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition, Speed);
        if (Vector2.Distance(transform.position, TargetPosition) < Mathf.Epsilon)
        {
            Destroy(gameObject);
            Instantiate(destroyVFX, gameObject.transform.position, destroyVFX.transform.rotation);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
        }
    }
}
