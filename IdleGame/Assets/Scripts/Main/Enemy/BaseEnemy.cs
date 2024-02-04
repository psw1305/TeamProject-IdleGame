using System.Collections;
using TMPro;
using UnityEngine;

public class BaseEnemy : ObjectPoolable, IDamageable
{
    [SerializeField] private Canvas UICanvas;

    #region Fields

    private EnemyBlueprint _enemyBlueprint;
    private EnemyView _enemyView;

    private string _enemyName;
    private Coroutine _attackCoroutine;

    //체력
    private long _maxHp;
    private long _currentHP;

    //공격
    private long _damage;
    private float _attackSpeed;
    private float _range;

    //속도
    private float _moveSpeed;

    public long _rewards;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    public int TestWeight;

    #endregion

    #region Init

    public void SetEnemy(EnemyBlueprint blueprint, Vector2 position, int statWeight, int goldWeight)
    {
        _enemyBlueprint = blueprint;
        _enemyName = blueprint.EnemyName;
        _spriteRenderer.sprite = blueprint.EnemySprite;

        _maxHp = blueprint.HP;


        _damage = blueprint.Damage;
        _attackSpeed = blueprint.AttackSpeed;
        _range = blueprint.Range;

        _moveSpeed = blueprint.MoveSpeed;
        _rewards = blueprint.Rewards;

        gameObject.name = _enemyName;

        SetPosition(position);
        SetStatWeight(statWeight);
        SetGoldWeight(goldWeight);
        ResetHealth();
        SetHpBar();
    }

    public void SetStatWeight(int Weight)
    {
        long _weight = (long)(Weight - 1);
        _maxHp = _maxHp + _maxHp * _weight;
        _damage = _damage + _damage * _weight;
        ResetHealth();
    }

    public void SetGoldWeight(int Weight)
    {
        long _weight = (long)(Weight - 1);
        _rewards = _rewards + _rewards * _weight;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetHpBar()
    {
        _enemyView.SetHpBar(_enemyBlueprint.EnemyType);
        _enemyView.SetHealthBar(GetCurrentHpPercent(), _currentHP, true);
    }

    //추후 오브젝트 풀링 시 초기화 할 수 있게 메서드
    private void ResetHealth()
    {
        _currentHP = _maxHp;
    }
    #endregion

    #region Unity Flow

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyView = GetComponent<EnemyView>();
    }

    private void FixedUpdate()
    {
        EvaluateState();
    }

    #endregion

    #region StateMethod
    //플레이어 방향으로 이동
    private void EvaluateState()
    {
        if (_range < Vector2.Distance(Manager.Game.Player.gameObject.transform.position, transform.position))
        {
            _rigidbody.velocity = new Vector2(
                Manager.Game.Player.gameObject.transform.position.x - transform.position.x,
                0f
                ).normalized * _moveSpeed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;

            _attackCoroutine ??= StartCoroutine(AttackRoutine());
            //if (_attackCoroutine == null)
            //{
            //    _attackCoroutine = StartCoroutine(AttackRoutine());
            //}
        }
    }
    #endregion

    #region Attack Method
    //발사체를 생성 및 초기화
    private void CreateProjectail()
    {
        //Resources 폴더에서 EnemyProjectileFrame(발사체 틀)을 생성하고 go로 할당받음
        //var go = Manager.ObjectPool.GetGo("EnemyProjectileFrame");
        var go = Manager.Resource.InstantiatePrefab("EnemyProjectileFrame");
        go.transform.position = gameObject.transform.position;


        //발사체 초기화를 위해 정보를 넘겨줌
        go.GetComponent<EnemyProjectileHandler>().ProjectileVFX = _enemyBlueprint.ProjectailVFX;
        go.GetComponent<EnemyProjectileHandler>().Damage = _damage;
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

    public void FloatingDamage(Vector3 position, long damage, DamageType damageTypeValue)
    {
        GameObject DamageHUD = Manager.Resource.InstantiatePrefab("Canvas_FloatingDamage");
        //GameObject DamageHUD = Manager.ObjectPool.GetGo("Canvas_FloatingDamage");
        //GameObject DamageHUD = Manager.ObjectPool.GetGo("FloatingText");
        //DamageHUD.transform.SetParent(UICanvas.transform, true);
        DamageHUD.GetComponent<UIFloatingText>().Initialize();
        DamageHUD.GetComponent<UIFloatingText>().Alpha = damageTypeValue == DamageType.Critical ? Color.red : Color.white;
        DamageHUD.transform.position = this.gameObject.transform.position + position;
        DamageHUD.GetComponent<UIFloatingText>().SetDamage(damage);
    }

    #endregion

    #region Health Method

    public void TakeDamage(long damage, DamageType damageTypeValue)
    {        
        AmountDamage(damage);
        FloatingDamage(new Vector3(0, 0.05f, 0), damage, damageTypeValue);
        _enemyView.SetHealthBar(GetCurrentHpPercent(), _currentHP);
    }

    private float GetCurrentHpPercent()
    {
        return (float)_currentHP / _maxHp;
    }

    private void AmountDamage(long damage)
    {
        if (_currentHP - damage <= 0)
        {
            _currentHP = 0;
            Manager.Stage.GetEnemyList().Remove(gameObject.GetComponent<BaseEnemy>());
            Die();
        }
        else
        {
            _currentHP -= damage;            
        }
    }

    private void Die()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        Manager.Game.Player.RewardGold(_rewards);

        if(Manager.Quest.CurrentQuest.questType == QuestType.DefeatEnemy)
        {
            Manager.Quest.QuestCurrentValueUp();
            UISceneMain uiSceneMain = Manager.UI.CurrentScene as UISceneMain;
            uiSceneMain.UpdateQuestObjective();
        }

        _attackCoroutine = null;
        Destroy(gameObject);
        _enemyView.ClearHpBar();
        //ReleaseObject();
    }

    #endregion
}
