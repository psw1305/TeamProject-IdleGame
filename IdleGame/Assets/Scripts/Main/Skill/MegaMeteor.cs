using System.Collections;
using UnityEngine;

public class MegaMeteor : BaseSkill
{
    private string _skillID = "S0013";

    private long _damage;
    private DamageType _damageType;

    [SerializeField] private GameObject projectileSpawnArea;
    [SerializeField] private GameObject skillProjectile;

    [SerializeField] private Vector2 minDestinationPosition;
    [SerializeField] private Vector2 maxDestinationPosition;

    private MeteorProjectile _projectile;
    private Coroutine _atkCor;

    protected override void ApplySkillEffect()
    {

        _atkCor = StartCoroutine(AtkLoop());
        _skillDamageRatio = CalculateDamageRatio(_skillID);
        Manager.Game.Player.FinalAttackDamage(out _damage, out _damageType);
    }

    protected override void RemoveSkillEffect()
    {
        if (_atkCor != null)
        {
            StopCoroutine(_atkCor);
            _atkCor = null;
        }
    }

    IEnumerator AtkLoop()
    {
        while (true)
        {
            if (_player.State == PlayerState.Battle)
            {
                _projectile = Instantiate(skillProjectile).GetComponent<MeteorProjectile>();

                _projectile.Damage = (long)(_damage * _skillDamageRatio);
                _projectile.DamageTypeValue = _damageType;

                _projectile.transform.position = new Vector2(0, 5);
                _projectile.TargetPosition = new Vector2(Random.Range(minDestinationPosition.x, maxDestinationPosition.x), Random.Range(minDestinationPosition.y, maxDestinationPosition.y));
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
