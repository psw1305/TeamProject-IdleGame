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

    //public UpgradeInfo UpgradeHp;
    //public UpgradeInfo UpgradeHpRecovery;
    //public UpgradeInfo UpgradeAttackDamage;
    //public UpgradeInfo UpgradeAttackSpeed;
    //public UpgradeInfo UpgradeCriticalChance;
    //public UpgradeInfo UpgradeCriticalDamage;

    public StatInfo HpInfo;
    public StatInfo HpRecoveryInfo;
    public StatInfo AttackDamageInfo;
    public StatInfo AttackSpeedInfo;
    public StatInfo CriticalChanceInfo;
    public StatInfo CriticalDamageInfo;

    private PlayerView playerView;
    public List<BaseEnemy> enemyList;
    private Rigidbody2D playerRigidbody;

    private Coroutine attackCoroutine;

    #endregion

    #region Properties

    public long CurrentHp { get; private set; }
    public long Hp { get; private set; }
    public long HpRecovery { get; private set; }
    public long AttackDamage { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CriticalChance { get; private set; }
    public float CriticalDamage { get; private set; }
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
        Hp = 1000;
        HpRecovery = 30;

        AttackDamage = 10;
        AttackSpeed = 0.50f;
        CriticalChance = 50.00f;
        CriticalDamage = 1000;

        AttackRange = 5;
        MoveSpeed = 100;

        HpInfo = new StatInfo(1, 50);
        HpRecoveryInfo = new StatInfo(1, 50);
        AttackDamageInfo = new StatInfo(1, 90);
        AttackSpeedInfo = new StatInfo(1, 10);
        CriticalChanceInfo = new StatInfo(1, 100);
        CriticalDamageInfo = new StatInfo(1, 100);

        SetCurrentHp(Hp);

        enemyList = Manager.Stage.GetEnemyList();

        Manager.Inventory.InitItem();
        EquipmentStatModifier();
        StartCoroutine(RecoverHealthPoint());
    }

    #endregion

    #region Stat Modifier

    public void HealthUp(long modifier)
    {
        Hp += modifier;
    }

    public void HealthRecoveryUp(long modifier)
    {
        HpRecovery += modifier;
    }

    public void AttackDamageUp(long modifier)
    {
        AttackDamage += modifier;
    }

    public void AttackSpeedUp(float modifier)
    {
        AttackSpeed += modifier;
        AttackSpeed = Mathf.Round((float)AttackSpeed * 100) / 100;
    }

    public void CriticalChanceUp(float modifier)
    {
        CriticalChance += modifier;
    }

    public void CriticalDamageUp(float modifier)
    {
        CriticalDamage += modifier;
    }

    public void EarnGold(long amount)
    {
        Gold += amount;
    }

    public void EarnGems(int amount)
    {
        Gems += amount;
    }

    public void UsedGold(long amount)
    {
        Gold -= amount;
    }

    public void UpgradeStat(StatInfo stat)
    {
        if (Gold < stat.UpgradeCost) return;

        UsedGold(stat.UpgradeCost);

        stat.Level++;
        stat.UpgradeCost += 50;

        CalculateStats();
    }

    private void CalculateStats()
    {
        Hp += 100;
        HpRecovery += 10;
        AttackDamage += 10;
        AttackSpeed += 0.01f;
        CriticalChance += 0.1f;
        CriticalDamage += 10f;
    }

    public void EquipmentStatModifier()
    {
        RetentionEffect = 0;
        EquipStat = 0;

        foreach (var item in Manager.Inventory.ItemDataBase.ItemDB)
        {
            RetentionEffect += item.retentionEffect + item.reinforceEffect * item.level;
        }

        var filteredEquipItem = Manager.Inventory.ItemDataBase.ItemDB.Where(itemdata => itemdata.equipped == true).ToList();

        foreach (var item in filteredEquipItem)
        {
            EquipStat += item.equipStat + item.reinforceEquip *item.level;
        }

        Debug.Log($"retentionEffect : {RetentionEffect}");
        Debug.Log($"equipStat : {EquipStat}");
    }

    #endregion

    #region Stat Methods

    private void SetCurrentHp(long amount)
    {
        CurrentHp = amount;
    }

    private float GetCurrentHpPercent()
    {
        return (float)CurrentHp / Hp;
    }

    private bool IsCritical()
    {
        float chance = Random.Range(1, 1001);

        if (chance < CriticalChance * 10)
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
            long criticalDamage = (long)(AttackDamage + (AttackDamage * CriticalDamage) * (1 + EquipStat / 100) * (1 + RetentionEffect / 100));
            Debug.Log($"치명타 : {criticalDamage}");
            return criticalDamage;
        }
        else
        {
            long damage = (long)(AttackDamage + (AttackDamage * CriticalDamage) * (1 + EquipStat / 100) * (1 + RetentionEffect / 100));
            Debug.Log($"치명타 : {damage}");
            return damage;
        }
    }

    private float AttakSpeedToTime()
    {
        return 1.0f / AttackSpeed;
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
        // TODO : 범위 내에 도달 시 공격
        //_playerRigidbody.velocity = Vector2.zero;
        //Attack(); // TODO : 공격이 Update로 계속 실행되서 projectile이 반복 생성되어 수정 필요함
        if (attackCoroutine == null && enemyList.Count > 0)
        {
            attackCoroutine = StartCoroutine(AttackRoutine());
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
        SetCurrentHp(Hp);
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

    #endregion

    #region Health Methods

    private IEnumerator RecoverHealthPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentHp = (long)Mathf.Clamp(CurrentHp + HpRecovery, 0, Hp);
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

    #region Reward Methods

    public void AmountGold(long Amount)
    {
        Gold = (long)Mathf.Clamp(Gold + Amount, 0, 1_000_000_000_000_000_000);
        
        UISceneMain uISceneTest = Manager.UI.CurrentScene as UISceneMain; // 변수화 
        uISceneTest.DisplayGold();
    }

    #endregion
}
