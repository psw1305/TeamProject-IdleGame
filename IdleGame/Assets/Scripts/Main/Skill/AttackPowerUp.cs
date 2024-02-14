
using UnityEngine;

public class AttackPowerUp : BaseSkill
{
    [SerializeField] private float amountValue;
    [SerializeField] private ParticleSystem skillVFX;

    protected override void Start()
    {
        base.Start();

        skillVFX.Stop();

        string _skillID = "S0002";
        amountValue =
            (Manager.SkillData.SkillDataDictionary[_skillID].skillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) * Manager.SkillData.SkillDataDictionary[_skillID].reinforceDamage)
            / 100;
    }
    protected override void ApplySkillEffect()
    {
        Manager.Game.Player.DamageBuff += amountValue;
        skillVFX.Play();
    }

    protected override void RemoveSkillEffect()
    {
        Manager.Game.Player.DamageBuff -= amountValue;
        skillVFX.Stop();
    }
}
