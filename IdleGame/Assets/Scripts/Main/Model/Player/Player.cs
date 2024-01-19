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

    private GameUserProfile profile;
    private PlayerView playerView;
    public List<BaseEnemy> enemyList;
    private Rigidbody2D playerRigidbody;
    private Coroutine attackCoroutine;

    private bool isClick = false;

    #endregion

    #region Properties

    public StatInfo AtkDamage { get; private set; }
    public StatInfo AtkSpeed { get; private set; }
    public StatInfo CritChance { get; private set; }
    public StatInfo CritDamage { get; private set; }
    public StatInfo Hp { get; private set; }
    public StatInfo HpRecovery { get; private set; }

    public long CurrentHp { get; private set; }
    public float AttackRange { get; private set; }
    public int MoveSpeed { get; private set; }
    public long Gold { get; private set; }
    public int Gems { get; private set; }

    // 장비 관련 프로퍼티
    public float EquipAttackStat { get; private set; }
    public float RetentionAttackEffect {  get; private set; }
    public float EquipHPStat { get; private set; }
    public float RetentionHPEffect { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        AttackRange = 5;
        MoveSpeed = 100;

        profile = Manager.Data.Profile;

        AtkDamage   = new StatInfo(profile.Stat_Level_AtkDamage, BaseStat.AtkDamage, 10, StatModType.Integer);
        AtkSpeed    = new StatInfo(profile.Stat_Level_AtkSpeed, BaseStat.AtkSpeed, 10, StatModType.DecimalPoint);
        CritChance  = new StatInfo(profile.Stat_Level_CritChance, BaseStat.CritChance, 1, StatModType.Percent);
        CritDamage  = new StatInfo(profile.Stat_Level_CritDamage, BaseStat.CritDamage, 10, StatModType.Percent);
        Hp          = new StatInfo(profile.Stat_Level_Hp, BaseStat.Hp, 100, StatModType.Integer);
        HpRecovery  = new StatInfo(profile.Stat_Level_HpRecovery, BaseStat.HpRecovery, 10, StatModType.Integer);

        SetCurrentHp(Hp.Value);

        enemyList = Manager.Stage.GetEnemyList();

        Manager.Inventory.InitItem();
        Manager.Quest.InitQuest();
        EquipmentStatModifier();
        StartCoroutine(RecoverHealthPoint());
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

        if (chance < CritChance.Value)
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
            return (long)(AttackDamage.Value + (AttackDamage.Value * CriticalDamage.GetFloat()) * (1 + EquipAttackStat / 100) * (1 + RetentionAttackEffect / 100));
        }
        else
        {
            return (long)(AttackDamage.Value + (1 + EquipAttackStat / 100) * (1 + RetentionAttackEffect / 100));
        }
    }

    private float AttakSpeedToTime()
    {
        return 1.0f / AtkSpeed.GetFloat();
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

    public void RewardGem(int Amount)
    {
        Gems = Gems + Amount;
        playerView.SetGemAmout();
    }

    #endregion

    #region Equipment Methods

    public void EquipmentStatModifier()
    {
        RetentionAttackEffect = 0;
        EquipAttackStat = 0;
        RetentionHPEffect = 0;
        EquipHPStat = 0;

        foreach (var item in Manager.Inventory.ItemDataBase.ItemDB.Where(itemData => itemData.level > 1 || itemData.hasCount > 0).ToList())
        {
            if (item.statType == "attack")
            {
                RetentionAttackEffect += item.retentionEffect + item.reinforceEffect * item.level;
            }
            else if (item.statType == "hp")
            {
                RetentionHPEffect += item.retentionEffect + item.reinforceEffect * item.level;
            }
        }

        var filteredEquipItem = Manager.Inventory.ItemDataBase.ItemDB.Where(itemdata => itemdata.equipped == true).ToList();

        foreach (var item in filteredEquipItem)
        {
            if (item.statType == "attack" && item.equipped == true)
            {
                EquipAttackStat += item.equipStat + item.reinforceEquip * item.level;
            }
            else if (item.statType == "hp" && item.equipped == true)
            {
                EquipHPStat += item.equipStat + item.reinforceEquip * item.level;
            }
        }
    }

    #endregion
}
