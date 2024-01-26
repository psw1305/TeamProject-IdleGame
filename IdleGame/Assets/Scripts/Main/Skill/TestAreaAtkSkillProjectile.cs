using UnityEngine;

public class TestAreaAtkSkillProjectile : ProjectileHandlerBase
{

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition, Speed);
        if(Vector2.Distance(transform.position, TargetPosition) < Mathf.Epsilon)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
        }
    }

}
