using UnityEngine;

public class TestAreaAtkSkillProjectile : ProjectileHandlerBase
{
    public Vector2 TargetPosition;


    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition);
        if(Vector2.Distance(transform.position, TargetPosition) < Mathf.Epsilon)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(Damage, DamageTypeValue);
        }
    }

}
