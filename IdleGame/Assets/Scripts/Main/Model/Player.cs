using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    public List<BaseEnemy> _enemyList;
    private Rigidbody2D _playerRigidbody;
    private GameObject _enemy;

    #endregion

    #region Properties

    public int Damage { get; private set; }
    public int Hp { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CriticalPercent { get; private set; }
    public float CriticalDamage { get; private set; }
    public float Range { get; private set; }
    public int Speed { get; private set; }

    private Coroutine _attackCoroutine;

    #endregion

    #region Init

    private void Start()
    {
        Damage = 10;
        Hp = 1000;
        AttackSpeed = 0.50f;
        CriticalPercent = 0.00f;
        CriticalDamage = 0;

        Range = 5;
        Speed = 100;

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _enemyList = Manager.Stage.GetEnemyList();
    }

    #endregion

    private void FixedUpdate()
    {
        // TODO : 범위 내에 도달 시 공격
        //_playerRigidbody.velocity = Vector2.zero;
        //Attack(); // TODO : 공격이 Update로 계속 실행되서 projectile이 반복 생성되어 수정 필요함
        if (_attackCoroutine == null && _enemyList.Count > 0)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
        else if (_enemyList.Count == 0)
        {
            _attackCoroutine = null;
            StopCoroutine(AttackRoutine());
        }
    }

    #region State

    public void Idle()
    {
        _playerRigidbody.velocity = Vector2.right * Speed * Time.deltaTime;
    }

    public void Attack()
    {
        // 공격 projectile 생성
        var testProjectile = Manager.Resource.InstantiatePrefab("PlayerProjectileFrame", ProjectilePoint);

        _enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = _enemyList[0].transform.position;
        testProjectile.GetComponent<PlayerProjectileHandler>().Damage = Damage;
    }
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / AttackSpeed);
            if (_enemyList.Count > 0)
            {
                Attack();
            }
            else
            {
                break;
            }
        }
        _attackCoroutine = null;
    }


    public void TakeDamage(int Damage)
    {
        AmountDamage(Damage);
        Debug.Log($"{gameObject.name} : {Hp}");
    }

    private void AmountDamage(int Damage)
    {
        if (Hp - Damage <= 0)
        {
            Hp = 0;
            Die();
        }
        else
        {
            Hp -= Damage;
        }
    }
    private void Die()
    {
        //이전 스테이지로
    }
    #endregion
}
