using UnityEngine;

public class Destruction : BaseSkill
{
    private string _skillID = "S0014";

    private long _damage;

    private DamageType _damageType;

    [SerializeField] private ParticleSystem vfxParticle;

    protected override void Start()
    {
        base.Start();
        vfxParticle.Stop();
    }

    protected override void ApplySkillEffect()
    {
        Manager.Game.Player.FinalAttackDamage(out _damage, out _damageType);
        _skillDamageRatio = CalculateDamageRatio(_skillID);
        vfxParticle.Play();
        for(int i = 0; i < Manager.Game.Player.enemyList.Count; i++)
        {
            Manager.Game.Player.enemyList[i].TakeDamage((long)(_damage * _skillDamageRatio), _damageType);
        }
    }

    protected override void RemoveSkillEffect()
    {
        vfxParticle.Stop();
    }
}
