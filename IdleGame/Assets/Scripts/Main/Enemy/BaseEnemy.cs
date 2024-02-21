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

    private bool _inViewport;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Camera _camera;

    private Coroutine _attackCoroutine;
    public int TestWeight;

    #endregion

    #region Init

    public void SetEnemy(EnemyBlueprint blueprint, Vector2 position, long hpWeight, long atkWeight, long goldWeight)
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

        //gameObject.name = _enemyName;

        SetPosition(position);
        SetStatWeight(hpWeight, atkWeight);
        SetGoldWeight(goldWeight);
        ResetHealth();
        SetHpBar();
    }

    public void SetStatWeight(long hpWeight, long atkWeight)
    {
        long _hpWeight = hpWeight - 1;
        _maxHp = _maxHp + _maxHp * _hpWeight;
        long _atkWeight = atkWeight - 1;
        _damage = _damage + _damage * _atkWeight;
        ResetHealth();
    }

    public void SetGoldWeight(long Weight)
    {
        long _weight = Weight - 1;
        _rewards = _rewards + _rewards * _weight;
    }

    public void SetPosition(Vector2 position)
    {
        float ratio = Manager.Game.screenRatio;
        position.x -= ratio;
        position.y -= ratio;
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
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 viewport = _camera.WorldToViewportPoint(transform.position);
        if ((0 < viewport.x & viewport.x < 1) && (0 < viewport.y & viewport.y < 1))
        {
            _inViewport = true;
        }
        else
        {
            _inViewport = false;
        }
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
        //사거리보다 거리가 멀거나 뷰포트에 들어오지 않았을때
        if (_range < Vector2.Distance(Manager.Game.Player.gameObject.transform.position, transform.position) | !_inViewport)
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
        }
    }
    #endregion

    #region Attack Method
    //발사체를 생성 및 초기화
    private void CreateProjectail()
    {
        //Resources 폴더에서 EnemyProjectileFrame(발사체 틀)을 생성하고 go로 할당받음
        var go = Manager.ObjectPool.GetGo("EnemyProjectileFrame");
        go.transform.position = gameObject.transform.position;

        //발사체 초기화를 위해 정보를 넘겨줌
        go.GetComponent<EnemyProjectileHandler>().SetProjectile(_enemyBlueprint.ProjectailVFX, _damage);
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
        GameObject DamageHUD = Manager.ObjectPool.GetGo("Canvas_FloatingDamage");
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
        if (_currentHP == 0)
        {
            return;
        }
        if (!_inViewport)
        {
            return;
        }
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
        //Destroy(gameObject);
        _enemyView.ClearHpBar();
        ReleaseObject();
    }

    #endregion
}
