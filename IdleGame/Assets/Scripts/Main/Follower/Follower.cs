using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    private PlayerFollowerHandler _followerHandler;
    private Coroutine _attackCoroutine;
    private FollowerAnimController _followerAnimController;
    [HideInInspector] public List<BaseEnemy> enemyList;
    private Player _player;

    private FollowerBlueprint _followerBlueprint;
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
    public float AtkCorrection { get; private set; }
    public float AtkSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public float CriticalChance { get; private set; }
    public long RetentionEffect { get; private set; }

    #endregion

    #region Init

    public void Initialize(FollowerBlueprint followerBlueprint)
    {
        _player = Manager.Game.Player;
        _followerBlueprint = followerBlueprint;

        _sprite.sprite = followerBlueprint.Sprite;
        _animator.runtimeAnimatorController = followerBlueprint.Animator;
        _rarity = followerBlueprint.Rarity.ToString();

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
        _followerHandler = Manager.Game.Player.GetComponent<PlayerFollowerHandler>();
        _followerHandler.AddFollowerWalkEvent(Walk);
        _followerHandler.AddFollowerBattleEvent(Battle);
    }

    #endregion

    public void Walk()
    {
        _followerAnimController.OnWalk();
    }

    public void Battle()
    {
        _followerAnimController.OnIdle();
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        while (enemyList.Count > 0)
        {
            if (Vector2.Distance(transform.position, enemyList[0].transform.position) <= AttackRange)
            {
                Attack();
            }
            yield return new WaitForSeconds(AttakSpeedToTime());
        }
        _attackCoroutine = null;
    }

    public void Attack()
    {
        _followerAnimController.OnRangeAtk();
        MakeRangeProjectile();
    }

    public void MakeRangeProjectile()
    {
        var testProjectile = Manager.ObjectPool.GetGo("FollowerProjectileFrame");
        testProjectile.transform.position = ProjectilePoint.position;
        //testProjectile.GetComponent<FollowerProjectileHandler>().ProjectileVFX.GetComponent<SpriteRenderer>().sprite = _followerBlueprint.ProjectileSprite; 
        enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = enemyList[0].transform.position;
        FinalAttackDamage(out testProjectile.GetComponent<PlayerProjectileHandler>().Damage, out testProjectile.GetComponent<PlayerProjectileHandler>().DamageTypeValue);
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
                * (1 + _player.CritDamage.GetIntegerFloat()));
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
        return 1.0f / Manager.Game.Player.AtkSpeed.GetFloat() / AtkSpeed;
    }
    private void OnDestroy()
    {
        if (_followerHandler != null)
        {
            _followerHandler.RemoveFollowerWalkEvent(Walk);
            _followerHandler.RemoveFollowerBattleEvent(Battle);
        }
    }
}