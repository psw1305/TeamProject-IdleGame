using System.Collections;
using System.Linq;
using UnityEngine;

public class Blizzard : BaseSkill
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
        vfxParticle.Play();
        _loopSkill = StartCoroutine(LoopSkillEffect());
    }

    protected override void RemoveSkillEffect()
    {
        vfxParticle.Stop();
        StopCoroutine(_loopSkill);
    }

    private void CalculateDamageRatio()
    {
        string _skillID = "S0008";
        _skillDamageRatio = (Manager.SkillData.SkillDataDictionary[_skillID].skillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) + Manager.SkillData.SkillDataDictionary[_skillID].reinforceDamage)
            / 100;
    }

    IEnumerator LoopSkillEffect()
    {
        CalculateDamageRatio();
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var enemy in Manager.Game.Player.enemyList.ToList())
            {
                Manager.Game.Player.FinalAttackDamage(out _damage, out _damageType);
                enemy.GetComponent<IDamageable>().TakeDamage((long)(_damage * _skillDamageRatio), _damageType);
            }
        }
    }
}
