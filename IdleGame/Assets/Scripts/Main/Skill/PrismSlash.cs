using UnityEngine;

public class PrismSlash : BaseSkill
{
    private string _skillID = "S0010";

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

        var go = Manager.Game.Player.enemyList[Random.Range(0, Manager.Game.Player.enemyList.Count)];
        go.TakeDamage((long)(_damage * _skillDamageRatio), _damageType);

        gameObject.transform.position = go.transform.position;
        vfxParticle.Play();
    }

    protected override void RemoveSkillEffect()
    {
        vfxParticle.Stop();
    }
}
