using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Follower : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    private Coroutine _attackCoroutine;
    private FollowerAnimController _followerAnimController;
    [HideInInspector] public List<BaseEnemy> enemyList;
    private Player _player;

    private EnemyBlueprint _followerBlueprint;
    private string _itemID;
    private string _followerName;
    private GameObject _followerPrefab;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private string _rarity;

    #endregion

    #region Properties
    // TODO: 플레이어의 데이터를 가져와서 가공하고 장착 시 보일 수 있도록

    public long AtkDamage { get; private set; }
    public float AtkCorrection {  get; private set; }
    public float AtkSpeed { get; private set; }
    public float AttackRange { get; private set; } 
    public float CriticalChance { get; private set; }
    public long RetentionEffect { get; private set; }

    #endregion

    #region Init

    public void Initialize(FollowerBlueprint followerBlueprint)
    {
        _player = Manager.Game.Player;

        _sprite.sprite = followerBlueprint.Sprite;
        _animator.runtimeAnimatorController = followerBlueprint.Animator;
        _rarity = followerBlueprint.Rarity;

        AttackRange = 4;
        AtkDamage = _player.AtkDamage.Value;
        AtkCorrection = followerBlueprint.DamageCorrection;
        AtkSpeed = followerBlueprint.AtkSpeed;
        
        enemyList = Manager.Stage.GetEnemyList();
    }

    #endregion

    #region Unity Flow

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _followerAnimController = GetComponent<FollowerAnimController>();
    }

    private void FixedUpdate()
    {
        if (_attackCoroutine == null && enemyList.Count <= 0)
        {
            _followerAnimController.OnWalk();
        }
        else if (_attackCoroutine == null && enemyList.Count > 0 && Vector2.Distance(enemyList[0].transform.position, transform.position) < 4)
        {
            _followerAnimController.OnIdle();
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    #endregion

   

    public void Idle()
    {
        // 애니메이션 OnIdle()
        // 이동시에 위치는 고정시키고 달리는 애니메이션만 실행
    }

    // 플레이어의 어택 코루틴이 들어가면 동료들도 공격 코루틴에 들어간다
    public void Attack()
    {
        _followerAnimController.OnRangeAtk();
        MakeRangeProjectile();
    }

    public void MakeRangeProjectile()
    {
        var testProjectile = Manager.ObjectPool.GetGo("FollowerProjectileFrame");
        testProjectile.transform.position = ProjectilePoint.position;
        enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = enemyList[0].transform.position;
        FinalAttackDamage(out testProjectile.GetComponent<PlayerProjectileHandler>().Damage, out testProjectile.GetComponent<PlayerProjectileHandler>().DamageTypeValue);
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(AttakSpeedToTime());
            if (enemyList.Count == 0 || Vector2.Distance(transform.position, enemyList[0].transform.position) > AttackRange)
            {
                _attackCoroutine = null;
                _followerAnimController.OnWalk();
                break;
            }

            if (Vector2.Distance(transform.position, enemyList[0].transform.position) <= AttackRange)
            {
                Attack();
            }
        }
    }

    private bool IsCritical()
    {
        int chance = Random.Range(1, 1001);

        if (chance < _player.CritChance.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // TODO : 크리티컬 확률 추가, 캐릭터 데미지 가져와서 계산식 사요하기
    private void FinalAttackDamage(out long damage, out DamageType damageTypeValue)
    {
        if (IsCritical())
        {
            damage = (long)((_player.AtkDamage.Value * AtkCorrection)
                * (1 + _player.CritDamage.GetFloat()));
            damageTypeValue = DamageType.Critical;
        }
        else
        {
            damage = (long)(_player.AtkDamage.Value * AtkCorrection);
            damageTypeValue = DamageType.Normal;
        }
    }

    private float AttakSpeedToTime()
    {
        return 1.0f / AtkSpeed;
    }
}