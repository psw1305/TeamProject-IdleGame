
using UnityEngine;

public class TestBuffSkill : BaseSkill, ISkillUsingEffect
{
    [SerializeField] private float amountValue;
    [SerializeField] private GameObject skillVFX;

    private GameObject _currentVFX;
    public void ApplySkillEffect()
    {
        Manager.Game.Player.DamageBuff += amountValue;
        _currentVFX = Manager.Resource.InstantiatePrefab("BuffSkillVFX");
    }

    public void RemoveSkillEffect()
    {
        Manager.Game.Player.DamageBuff -= amountValue;
        Destroy(_currentVFX);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (Manager.Game.Player.enemyList.Count > 0)
            {
                UseSkill(this);
            }
        }
    }

}
