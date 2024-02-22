using System.Collections;
using System.Linq;
using UnityEngine;

public class Blizzard : BaseSkill
{
    private string _skillID = "S0008";

    private long _damage;

    private DamageType _damageType;
    private Coroutine _loopSkill;

    [SerializeField] private ParticleSystem vfxParticle;
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
        if (_loopSkill != null)
        {
            StopCoroutine(_loopSkill);
        }
    }

    IEnumerator LoopSkillEffect()
    {
        _skillDamageRatio = CalculateDamageRatio(_skillID);
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            foreach (var enemy in Manager.Game.Player.enemyList.ToList())
            {
                Manager.Game.Player.FinalAttackDamage(out _damage, out _damageType);
                enemy.TakeDamage((long)(_damage * _skillDamageRatio), _damageType);
            }
        }
    }
}
