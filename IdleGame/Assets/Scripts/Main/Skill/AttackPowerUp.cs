using UnityEngine;

public class AttackPowerUp : BaseSkill
{
    string _skillID = "S0002";

    private float _amountValue;

    [SerializeField] private ParticleSystem skillVFX;

    protected override void Start()
    {
        base.Start();

        skillVFX.Stop();
    }
    protected override void ApplySkillEffect()
    {
        _amountValue = CalculateDamageRatio(_skillID);
        Manager.Game.Player.DamageBuff += _amountValue;
        skillVFX.Play();
    }

    protected override void RemoveSkillEffect()
    {
        Manager.Game.Player.DamageBuff -= _amountValue;
        skillVFX.Stop();
    }
}
