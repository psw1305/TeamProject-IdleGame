using UnityEngine;

public class FireBallProjectile : ProjectileHandlerBase
{
    private GameObject _enemyObj;
    private float _skillDamageRatio;
    protected override void Start()
    {
        string _skillID = "S0001";
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
        if(TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            _enemyObj = collision.gameObject;
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (_enemyObj != null)
        {
            _enemyObj.GetComponent<IDamageable>().TakeDamage((long)(Damage * _skillDamageRatio), DamageTypeValue);
        }    
    }
}
