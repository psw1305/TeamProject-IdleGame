using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;

    #endregion

    #region Fields

    private PlayerView playerView;
    public List<BaseEnemy> enemyList;
    private Rigidbody2D playerRigidbody;
    private Coroutine attackCoroutine;

    private StatInfo CurrentStat;
    private bool isClick = false;

    #endregion

    #region Properties

    public StatInfo Hp { get; private set; }
    public StatInfo HpRecovery { get; private set; }
    public StatInfo AttackDamage { get; private set; }
    public StatInfo AttackSpeed { get; private set; }
    public StatInfo CriticalChance { get; private set; }
    public StatInfo CriticalDamage { get; private set; }

    public long CurrentHp { get; private set; }
    public float AttackRange { get; private set; }
    public int MoveSpeed { get; private set; }
    public long Gold { get; private set; }
    public int Gems { get; private set; }

    // 장비 관련 프로퍼티
    public float EquipStat { get; private set; }
    public float RetentionEffect {  get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        AttackRange = 5;
        MoveSpeed = 100;

        Hp = new StatInfo(1, 1000, 50, 100, StatModType.Integer);
        HpRecovery = new StatInfo(1, 30, 50, 10, StatModType.Integer);
        AttackDamage = new StatInfo(1, 10, 50, 10, StatModType.Integer);
        AttackSpeed = new StatInfo(1, 500, 50, 10, StatModType.DecimalPoint);
        CriticalChance = new StatInfo(1, 500, 50, 1, StatModType.Percent);
        CriticalDamage = new StatInfo(1, 1000, 50, 10, StatModType.Percent);

        SetCurrentHp(Hp.Value);

        enemyList = Manager.Stage.GetEnemyList();

        Manager.Inventory.InitItem();
        EquipmentStatModifier();
        StartCoroutine(RecoverHealthPoint());
    }

    public void SetCurrentStat(StatInfo statInfo)
    {
        CurrentStat = statInfo;
    }

    public void CheckClick(bool isClick)
    {
        this.isClick = isClick;
    }

    #endregion

    #region Unity Flow

    private void Start()
    {
        playerView = GetComponent<PlayerView>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (attackCoroutine == null && enemyList.Count > 0)
        {
            attackCoroutine = StartCoroutine(AttackRoutine());
        }

        if (isClick)
        {
            //Debug.Log("Click Down");
        }
    }

    #endregion

    #region State

    public void Idle()
    {
        playerRigidbody.velocity = MoveSpeed * Time.deltaTime * Vector2.right;
    }

    private void Dead()
    {
        //이전 스테이지로, Hp 리셋
        Manager.Stage.StageFailed();
        SetCurrentHp(Hp.Value);
    }

    #endregion

    #region Health Methods

    private void SetCurrentHp(long amount)
    {
        CurrentHp = amount;
    }

    private float GetCurrentHpPercent()
    {
        return (float)CurrentHp / Hp.Value;
    }

    private IEnumerator RecoverHealthPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentHp = (long)Mathf.Clamp(CurrentHp + HpRecovery.Value, 0, Hp.Value);
            playerView.SetHealthBar(GetCurrentHpPercent());
        }
    }

    public void TakeDamage(long Damage)
    {
        PlayerDamaged(Damage);
        FloatingDamage(new Vector3(0, 0.25f, 0), Damage);
        playerView.SetHealthBar(GetCurrentHpPercent());
    }

    private void PlayerDamaged(long damage)
    {
        if (CurrentHp - damage <= 0)
        {
            CurrentHp = 0;
            Dead();
        }
        else
        {
            CurrentHp -= damage;
        }
    }

    public void FloatingDamage(Vector3 position, long Damage)
    {
        playerView.SetDamageFloating(position, Damage);
    }

    #endregion

    #region Attack Method

    public void Attack()
    {
        // 공격 projectile 생성
        var testProjectile = Manager.Resource.InstantiatePrefab("PlayerProjectileFrame", ProjectilePoint);
        enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = enemyList[0].transform.position;
        testProjectile.GetComponent<PlayerProjectileHandler>().Damage = FinalAttackDamage();
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(AttakSpeedToTime());

            if (enemyList.Count == 0)
            {
                attackCoroutine = null;
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

        if (chance < CriticalChance.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private long FinalAttackDamage()
    {
        if (IsCritical())
        {
            return (long)(AttackDamage.Value + (AttackDamage.Value * CriticalDamage.GetFloat()) * (1 + EquipStat / 100) * (1 + RetentionEffect / 100));
        }
        else
        {
            return (long)(AttackDamage.Value + (1 + EquipStat / 100) * (1 + RetentionEffect / 100));
        }
    }

    private float AttakSpeedToTime()
    {
        return 1.0f / AttackSpeed.GetFloat();
    }

    #endregion

    #region Currenct Methods

    public bool IsTrade(long amount)
    {
        if (Gold - amount < 0)
        {
            return false;
        }
        else
        {
            Gold -= amount;
            playerView.SetGoldAmount();
            return true;
        }
    }

    public void RewardGold(long Amount)
    {
        Gold = (long)Mathf.Clamp(Gold + Amount, 0, 1_000_000_000_000_000_000);
        playerView.SetGoldAmount();
    }

    #endregion

    #region Equipment Methods

    public void EquipmentStatModifier()
    {
        RetentionEffect = 0;
        EquipStat = 0;

        foreach (var item in Manager.Inventory.ItemDataBase.ItemDB.Where(itemData => itemData.level > 1 || itemData.hasCount > 0).ToList())
        {
            RetentionEffect += item.retentionEffect + item.reinforceEffect * item.level;
        }

        var filteredEquipItem = Manager.Inventory.ItemDataBase.ItemDB.Where(itemdata => itemdata.equipped == true).ToList();

        foreach (var item in filteredEquipItem)
        {
            EquipStat += item.equipStat + item.reinforceEquip * item.level;
        }

        Debug.Log($"EffectStat : {RetentionEffect}");
        Debug.Log($"EquipStat : {EquipStat}");
    }

    #endregion
}
