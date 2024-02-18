using System.Collections;
using UnityEngine;

public class FireBall : BaseSkill
{
    [SerializeField] private GameObject projectileSpawnPosition;

    private GameObject _projectile;
    private Coroutine _atkCor;

    protected override void ApplySkillEffect()
    {
        _atkCor = StartCoroutine(AtkLoop());
    }

    protected override void RemoveSkillEffect()
    {
        StopCoroutine(_atkCor);
        _atkCor = null;
    }

    IEnumerator AtkLoop()
    {
        while (Manager.Game.Player.enemyList.Count > 0)
        {
            _projectile = Manager.Asset.InstantiatePrefab("FireBallProjectile", transform);
            _projectile.transform.position = projectileSpawnPosition.transform.position;

            Manager.Game.Player.FinalAttackDamage(out _projectile.GetComponent<FireBallProjectile>().Damage
                , out _projectile.GetComponent<FireBallProjectile>().DamageTypeValue);

            _projectile.GetComponent<FireBallProjectile>().TargetPosition = Manager.Game.Player.enemyList[0].transform.position;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
