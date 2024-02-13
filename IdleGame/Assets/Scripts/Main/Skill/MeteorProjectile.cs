using UnityEngine;

public class MeteorProjectile : ProjectileHandlerBase
{
    [SerializeField] private GameObject destroyVFX;
    private float _skillDamageRatio;
    protected override void Start()
    {
        string _skillID = "S0009";
        _skillDamageRatio = (Manager.SkillData.SkillDataDictionary[_skillID].skillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) + Manager.SkillData.SkillDataDictionary[_skillID].reinforceDamage)
            / 100;
    }

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition, Speed);
        if (Vector2.Distance(transform.position, TargetPosition) < Mathf.Epsilon)
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
            collision.gameObject.GetComponent<IDamageable>().TakeDamage((long)(Damage * _skillDamageRatio), DamageTypeValue);
        }
    }

    private void OnDestroy()
    {
        if (gameObject != null)
        {
            Instantiate(destroyVFX, gameObject.transform.position, destroyVFX.transform.rotation);
        }
    }
}
