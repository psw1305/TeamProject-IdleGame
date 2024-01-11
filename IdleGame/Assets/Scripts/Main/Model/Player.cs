using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    private List<GameObject> _enemyList;
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
    public int Speed {  get; private set; }

    #endregion

    #region Init

    private void Start()
    {
        Damage = 10;
        Hp = 10;
        AttackSpeed = 0.10f;
        CriticalPercent =  0.00f;
        CriticalDamage = 0;

        Range = 2;
        Speed = 100;

        _playerRigidbody = GetComponent<Rigidbody2D>();

        // 임시로 리스트 안에 넣기 
        _enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Respawn"));
        //_enemy = _enemyList[0];

    }

    #endregion

    private void FixedUpdate()
    {
        //적이 범위 밖에 있으면
        //if (Range < Vector2.Distance(transform.position, _enemyList[0].transform.position))
        //{
        //    // 평소 달리기 상태로 변경
        //    Idle();
        //}
        //else // 범위 안에 있으면 정지 후 공격
        //{
        //    _playerRigidbody.velocity = Vector2.zero;
        //    Attack(); // TODO : 공격이 Update로 계속 실행되서 projectile이 반복 생성되어 수정 필요함
        //}
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
