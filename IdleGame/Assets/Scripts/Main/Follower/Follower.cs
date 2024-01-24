using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    private Coroutine _attackCoroutine;
    private FollowerAnimController _followerAnimController;


    #endregion

    private void Start()
    {
        _followerAnimController = GetComponent<FollowerAnimController>();
    }

    #region Properties

    public long AtkDamage { get; private set; }
    public float AtkSpeed { get; private set; }
    public long RetentionEffect {  get; private set; }

    #endregion

    public void Idle()
    {
        // 애니메이션 OnIdle()
        // 이동시에 위치는 고정시키고 달리는 애니메이션만 실행
    }

    // 플레이어의 어택 코루틴이 들어가면 동료들도 공격 코루틴에 들어간다
    public void Attack(List<BaseEnemy> enemyList)
    {
        _followerAnimController.OnRangeAtk();
        MakeRangeProjectile(enemyList);
    }

    public void MakeRangeProjectile(List<BaseEnemy> enemyList)
    {
        var testProjectile = Manager.Resource.InstantiatePrefab("PlayerProjectileFrame", ProjectilePoint);
        enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = enemyList[0].transform.position;
        FinalAttackDamage(out testProjectile.GetComponent<PlayerProjectileHandler>().Damage, out testProjectile.GetComponent<PlayerProjectileHandler>().DamageTypeValue);
    }

    private void FinalAttackDamage(out long damage, out DamageType damageTypeValue)
    {
        damage = AtkDamage;
        damageTypeValue = DamageType.Normal;
    }
}