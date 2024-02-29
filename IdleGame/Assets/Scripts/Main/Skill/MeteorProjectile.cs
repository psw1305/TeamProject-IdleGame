using UnityEngine;

public class MeteorProjectile : SkillProjectileHandlerBase
{
    [SerializeField] private GameObject destroyVFX;
    private float _skillDamageRatio;
    private void Start()
    {
        string _skillID = "S0009";
        _skillDamageRatio = (Manager.SkillData.SkillDataDictionary[_skillID].SkillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) + Manager.SkillData.SkillDataDictionary[_skillID].ReinforceDamage)
            / 100;
    }

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
            collision.gameObject.GetComponent<IDamageable>().TakeDamage((long)(Damage * _skillDamageRatio), DamageTypeValue);
        }
    }
}
