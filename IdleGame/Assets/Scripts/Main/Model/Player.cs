using System;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    private List<BaseEnemy> _enemyList;
    private Rigidbody2D _playerRigidbody;
    private GameObject _enemy;

    #endregion
    public struct UpgradeInfo
    {
        public int Level;
        public int UpgradCost;

        public UpgradeInfo(int Lv, int Cost)
        {
            this.Level = Lv;
            this.UpgradCost = Cost;
        }

        public void SetModifier(int Lv, int Cost)
        {
            this.Level += Lv;
            this.UpgradCost += Cost;
        }
    }

    #region Properties

    public long Damage { get; private set; }
    public long Hp { get; private set; }
    public double AttackSpeed { get; private set; }
    public float CriticalPercent { get; private set; }
    public float CriticalDamage { get; private set; }
    public float Range { get; private set; }
    public int Speed {  get; private set; }
    public UpgradeInfo DamageInfo ;
    public UpgradeInfo HpInfo;
    public UpgradeInfo AttackSpeedInfo;

    #endregion

    #region Init

    private void Start()
    {
        Damage = 10;
        Hp = 10;
        AttackSpeed = 0.10f;
        CriticalPercent =  0.00f;
        CriticalDamage = 0;

        Range = 5;
        Speed = 100;

        DamageInfo = new UpgradeInfo(1, 90);
        HpInfo = new UpgradeInfo(1, 50);
        AttackSpeedInfo = new UpgradeInfo(1, 200);

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _enemyList = Manager.Stage.GetEnemyList();
    }

    #endregion

    #region StatModifier

    public void DamageUp(int modifier)
    {
        Damage += modifier;
    }

    public void HpUp(int modifier)
    {
        Hp += modifier;
    }

    public void AttackSpeedUp(float modifier)
    {
        AttackSpeed += modifier;
        AttackSpeed = Mathf.Round((float)AttackSpeed * 100) / 100;
    }

    #endregion

    private void FixedUpdate()
    {
        // TODO : 범위 내에 도달 시 공격
        //_playerRigidbody.velocity = Vector2.zero;
        //Attack(); // TODO : 공격이 Update로 계속 실행되서 projectile이 반복 생성되어 수정 필요함
    }

    #region State

    public void Idle()
    {
        _playerRigidbody.velocity = Vector2.right * Speed * Time.deltaTime;
    }

    public void AttakingRepeat()
    {
        InvokeRepeating(nameof(Attack), 0, 10);
    }

    public void Attack()
    {
        // 공격 projectile 생성
        var testProjectile = Manager.Resource.InstantiatePrefab("IceProjectile", ProjectilePoint);
        testProjectile.transform.Translate(_enemyList[0].transform.position * Time.deltaTime);
    }

    #endregion
}
