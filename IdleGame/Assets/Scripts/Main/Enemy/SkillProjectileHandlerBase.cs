using UnityEngine;

public class SkillProjectileHandlerBase : MonoBehaviour
{
    [HideInInspector]
    public long Damage;

    [HideInInspector]
    public DamageType DamageTypeValue = DamageType.Normal;

    public float Speed = 0.1f;

    public Vector2 TargetPosition;

    public LayerMask TargetLayerMask;

    public SpriteRenderer projectileSprite;

    protected void TrackingTarget(Vector2 targetPosition, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
    }
}
