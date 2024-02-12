
using UnityEngine;

public class TestBuffSkill : BaseSkill
{
    [SerializeField] private float amountValue;
    [SerializeField] private GameObject skillVFX;

    private GameObject _currentVFX;
    protected override void Start()
    {
        base.Start();

        string _skillID = "S0002";
        amountValue =
            (Manager.SkillData.SkillDataDictionary[_skillID].skillDamage
            + (Manager.Data.SkillInvenDictionary[_skillID].level - 1) * Manager.SkillData.SkillDataDictionary[_skillID].reinforceDamage)
            / 100;
    }
    protected override void ApplySkillEffect()
    {
        Manager.Game.Player.DamageBuff += amountValue;
        _currentVFX = Manager.Address.InstantiatePrefab("BuffSkillVFX", gameObject.transform);
    }

    protected override void RemoveSkillEffect()
    {
        Manager.Game.Player.DamageBuff -= amountValue;
        Destroy(_currentVFX);
    }
}
