using System.Collections;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Fields

    private EnemyBlueprint _enemyBlueprint;

    private string _enemyName;
    private GameObject _projectileVFX;

    private Coroutine _attackCoroutine;
    private int _damage;
    private int _maxHp;
    private float _range;
    private float _attackSpeed;
    private int _rewards;
    private float _moveSpeed;

    private int _currentHP;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    #endregion

    #region Init

    public void SetEnemy(EnemyBlueprint blueprint)
    {
        _enemyBlueprint = blueprint;
        _enemyName = blueprint.EnemyName;
        _spriteRenderer.sprite = blueprint.EnemySprite;

        _maxHp = blueprint.HP;
        ResetHealth();

        _attackSpeed = blueprint.AttackSpeed;
        _range = blueprint.Range;

        _moveSpeed = blueprint.MoveSpeed;
        _rewards = blueprint.Rewards;

        gameObject.name = _enemyName;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    #endregion

    #region Unity Flow

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    #endregion

    #region Method
    //추후 오브젝트 풀링 시 초기화 할 수 있게 메서드
    private void ResetHealth()
    {
        _currentHP = _maxHp;
    }

    //플레이어 방향으로 이동
    private void OnMove()
    {
        if (_range < Vector2.Distance(Manager.Game.Player.gameObject.transform.position, transform.position))
        {
            _rigidbody.velocity = (Manager.Game.Player.gameObject.transform.position - transform.position).normalized * _moveSpeed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
            if (_attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackRoutine());
            }
        }
    }

    //발사체를 생성 및 초기화
    private void CreateProjectail()
    {
        //Resources 폴더에서 EnemyProjectileFrame(발사체 틀)을 생성하고 go로 할당받음
        var go = Manager.Resource.InstantiatePrefab("EnemyProjectileFrame", gameObject.transform);

        //발사체 초기화를 위해 정보를 넘겨줌
        go.GetComponent<ProjectileHandler>().ProjectileVFX = _enemyBlueprint.ProjectailVFX;
        go.GetComponent<ProjectileHandler>().Damage = _enemyBlueprint.Damage;
    }

    //발사체 생성 코루틴
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / _attackSpeed);
            CreateProjectail();
        }
    }

    private void ReceiveDamage(int Damage)
    {
        if (_currentHP - Damage <= 0)
        {
            _currentHP = 0;
            //보상이 구현되어야 함
        }
        else
        {
            _currentHP -= Damage;
        }
    }
    #endregion
}
