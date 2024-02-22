using System.Collections;
using UnityEngine;

public class FireBall : BaseSkill
{
    [SerializeField] private GameObject projectileSpawnPosition;
    [SerializeField] private GameObject skillProjectile;

    private FireBallProjectile _projectile;
    private Coroutine _atkCor;

    protected override void ApplySkillEffect()
    {
        _atkCor = StartCoroutine(AtkLoop());
    }

    protected override void RemoveSkillEffect()
    {
        if (_atkCor != null)
        {
            StopCoroutine(_atkCor);
        }
    }

    IEnumerator AtkLoop()
    {
        while (Manager.Game.Player.enemyList.Count > 0)
        {
            _projectile = Instantiate(skillProjectile, transform).GetComponent<FireBallProjectile>();
            _projectile.transform.position = projectileSpawnPosition.transform.position;

            Manager.Game.Player.FinalAttackDamage(out _projectile.Damage, out _projectile.DamageTypeValue);

            _projectile.TargetPosition = Manager.Game.Player.enemyList[0].transform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
