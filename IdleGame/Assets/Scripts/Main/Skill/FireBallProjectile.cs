using UnityEngine;

public class FireBallProjectile : SkillProjectileHandlerBase
{
    private GameObject _enemyObj;
    private float _skillDamageRatio;
    private void Start()
    {
        string _skillID = "S0001";
        _skillDamageRatio = (Manager.SkillData.SkillDataDictionary[_skillID].SkillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) + Manager.SkillData.SkillDataDictionary[_skillID].ReinforceDamage)
            / 100;
    }

    private void FixedUpdate()
    {
        TrackingTarget(TargetPosition, Speed);
        if (Vector2.Distance(transform.position, TargetPosition) < 0.02)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(TargetLayerMask.value == (TargetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            _enemyObj = collision.gameObject;
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_enemyObj != null)
        {
            _enemyObj.GetComponent<IDamageable>().TakeDamage((long)(Damage * _skillDamageRatio), DamageTypeValue);
        }    
    }
}
