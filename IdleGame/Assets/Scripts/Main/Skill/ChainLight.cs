using System.Collections;
using UnityEngine;

public class ChainLight : BaseSkill
{
    [SerializeField] private ParticleSystem vfxParticle;

    private long _damage;
    private float _skillDamageRatio;
    private DamageType _damageType;

    private Coroutine _loopSkill;
    protected override void Start()
    {
        base.Start();
        vfxParticle.Stop();
    }

    protected override void ApplySkillEffect()
    {
        _loopSkill = StartCoroutine(LoopSkill());
    }

    protected override void RemoveSkillEffect()
    {
        StopCoroutine(LoopSkill());
    }

    private void CalculateDamageRatio()
    {
        string _skillID = "S0003";
        _skillDamageRatio = (Manager.SkillData.SkillDataDictionary[_skillID].skillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) + Manager.SkillData.SkillDataDictionary[_skillID].reinforceDamage)
            / 100;
    }

    private IEnumerator LoopSkill()
    {
        vfxParticle.Play();

        CalculateDamageRatio();
        Manager.Game.Player.FinalAttackDamage(out _damage, out _damageType);

        for (int i = 0; i < Manager.Game.Player.enemyList.Count; i++)
        {
            var go = Manager.Game.Player.enemyList[i].gameObject;

            gameObject.transform.position = go.transform.position;
            go.GetComponent<IDamageable>().TakeDamage((long)(_damage * _skillDamageRatio), _damageType);

            yield return new WaitForSeconds(0.1f);
        }

        gameObject.transform.position = Manager.Game.Player.transform.position;
        vfxParticle.Stop();
    }
}
