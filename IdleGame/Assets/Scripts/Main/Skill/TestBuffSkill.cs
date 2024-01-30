
using UnityEngine;

public class TestBuffSkill : BaseSkill
{
    [SerializeField] private float amountValue;
    [SerializeField] private GameObject skillVFX;

    private GameObject _currentVFX;
    protected override void ApplySkillEffect()
    {
        Manager.Game.Player.DamageBuff += amountValue;
        _currentVFX = Manager.Resource.InstantiatePrefab("BuffSkillVFX", gameObject.transform);
    }

    protected override void RemoveSkillEffect()
    {
        Manager.Game.Player.DamageBuff -= amountValue;
        Destroy(_currentVFX);
    }
}
