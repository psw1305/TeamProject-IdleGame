using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;
    [SerializeField] private Transform[] FollowerPosition;

    #endregion

    #region Fields

    private GameUserProfile profile;
    private PlayerView playerView;
    private Coroutine _attackCoroutine;
    private float _damageBuff = 1;
    private event Action IdleRewardUpdate;

    [HideInInspector] public List<BaseEnemy> enemyList;

    // 동료 관련 프로퍼티
    private GameObject[] _followerPrefab = new GameObject[5];
    private Follower[] Follower = new Follower[5];

    // 플레이어 관련 스크립트
    private PlayerAnimController playerAnimController;
    private PlayerSkillHandler playerSkillHandler;
    private PlayerFollowerHandler playerFollowerHandler;

    private ParallaxController parallaxController;

    #endregion

    #region Properties

    // 테스트 용 플레이 제어
    public bool IsPlay = false;

    public PlayerState State = PlayerState.None;
    public StatInfo AtkDamage { get; private set; }
    public StatInfo AtkSpeed { get; private set; }
    public StatInfo CritChance { get; private set; }
    public StatInfo CritDamage { get; private set; }
    public StatInfo Hp { get; private set; }
    public StatInfo HpRecovery { get; private set; }

    public long ModifierHp { get; private set; }
    public long CurrentHp { get; private set; }
    public float AttackRange { get; private set; }
    public int MoveSpeed { get; private set; }
    public long Gold { get; private set; }
    public long ToTalIdleGold { get; private set; }     // 방치 골드 누적
    public int Gems { get; private set; }

    // 장비 관련 프로퍼티
    public float EquipAttackStat { get; private set; }
    public float RetentionAttackEffect { get; private set; }
    public float EquipHPStat { get; private set; }
    public float RetentionHPEffect { get; private set; }

    // 시간 관련 프로퍼티
    public int ToTalIdleTime { get; private set; }
    public DateTime IdleCheckTime { get; private set; }
    public DateTime BonusCheckTime { get; private set; }
    public bool IsBonusCheck { get; private set; }

    //버프 능력치 관련 프로퍼티
    public float DamageBuff
    {
        get => _damageBuff;
        set
        {
            if (value < 1) _damageBuff = 1;
            else _damageBuff = value;
        }
    }

    #endregion

    #region Init

    public void Initialize()
    {
        playerView = GetComponent<PlayerView>();
        playerAnimController = GetComponent<PlayerAnimController>();
        playerSkillHandler = GetComponent<PlayerSkillHandler>();
        playerFollowerHandler = GetComponent<PlayerFollowerHandler>();
        parallaxController = FindObjectOfType<ParallaxController>();

        AttackRange = 3;

        profile = Manager.Data.Profile;

        // 골드, 보석 데이터 적용
        Gold = profile.Gold;
        Gems = profile.Gems;

        // 스탯 데이터 적용
        AtkDamage = new StatInfo("Stat_Level_AtkDamage", profile.Stat_Level_AtkDamage, BaseStat.AtkDamage, 10, StatModType.Integer);
        AtkSpeed = new StatInfo("Stat_Level_AtkSpeed", profile.Stat_Level_AtkSpeed, BaseStat.AtkSpeed, 10, StatModType.DecimalPoint);
        CritChance = new StatInfo("Stat_Level_CritChance", profile.Stat_Level_CritChance, BaseStat.CritChance, 1, StatModType.Percent);
        CritDamage = new StatInfo("Stat_Level_CritDamage", profile.Stat_Level_CritDamage, BaseStat.CritDamage, 10, StatModType.Percent);
        Hp = new StatInfo("Stat_Level_Hp", profile.Stat_Level_Hp, BaseStat.Hp, 100, StatModType.Integer);
        HpRecovery = new StatInfo("Stat_Level_HpRecovery", profile.Stat_Level_HpRecovery, BaseStat.HpRecovery, 10, StatModType.Integer);

        enemyList = Manager.Stage.GetEnemyList();

        Manager.Inventory.InitItem();
        Manager.SkillData.InitSkill();
        Manager.FollowerData.InitFollower();
        Manager.Quest.InitQuest();
        EquipmentStatModifier();

        ModifierHp = (long)(Hp.Value + Hp.Value * (EquipHPStat / 100) + Hp.Value * (RetentionHPEffect / 100));
        SetCurrentHp(ModifierHp);

        IdleCheckTime = DateTime.ParseExact(profile.Date_Idle_ClickTime, "yyyy/MM/dd HH:mm:ss", null);
        BonusCheckTime = DateTime.ParseExact(profile.Date_Bonus_ClickTime, "yyyy/MM/dd HH:mm:ss", null);
        IsBonusCheck = profile.Date_Bonus_Check;

        //FollowerInit();

        playerSkillHandler.InitSkillSlot();
        playerFollowerHandler.InitFollowerSlot();

        StartCoroutine(RecoverHealthPoint());
    }

    private void InitIdleGoldReward(out int leftTime)
    {
        TimeSpan timeSpan = DateTime.Now - IdleCheckTime;
        int missedMinutes = Mathf.FloorToInt((float)timeSpan.TotalMinutes);
        ToTalIdleTime += missedMinutes;
        ToTalIdleGold += Manager.Stage.IdleGoldReward * missedMinutes;

        // 남은 시간 계산 후 lastRewardTime 갱신
        int secondsLeft = (int)(timeSpan.TotalSeconds % 60);
        IdleCheckTime = DateTime.Now.AddSeconds(-secondsLeft);
        leftTime = secondsLeft;
    }

    #endregion

    #region Unity Flow

    private void Update()
    {
        if (enemyList.Count == 0)
        {
            Walk();
        }
        else if (State != PlayerState.Battle & State != PlayerState.Die)
        {
            Battle();
        }
    }

    #endregion

    #region State

    public void Walk()
    {
        State = PlayerState.Move;
        playerAnimController.OnWalk();
        parallaxController.LayerMove();
        playerFollowerHandler.InvokeFollowerWalk();
    }

    public void Battle()
    {
        State = PlayerState.Battle;
        playerAnimController.OnIdle();
        playerFollowerHandler.InvokeFollowerBattle();
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    private void Dead()
    {
        //이전 스테이지로, Hp 리셋
        Manager.Stage.StageFailed();
        SetCurrentHp(ModifierHp);
        playerAnimController.OnDead();
        playerAnimController.OnRevive();
    }

    #endregion

    #region Health Methods

    private float GetCurrentHpPercent()
    {
        return (float)CurrentHp / ModifierHp;
    }

    private IEnumerator RecoverHealthPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CurrentHp = (long)Mathf.Clamp(CurrentHp + HpRecovery.Value, 0, ModifierHp);
            playerView.SetHealthBar(GetCurrentHpPercent());
        }
    }

    public void SetCurrentHp(long amount)
    {
        CurrentHp = amount;
    }

    public void TakeDamage(long Damage, DamageType damageTypeValue)
    {
        PlayerDamaged(Damage);
        FloatingDamage(new Vector3(0, 0.25f, 0), Damage, damageTypeValue);
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
            playerAnimController.OnHit();
            CurrentHp -= damage;
        }
    }

    public void FloatingDamage(Vector3 position, long Damage, DamageType damageTypeValue)
    {
        playerView.SetDamageFloating(position, Damage);
    }

    #endregion

    #region Attack Method

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
        // 효과음 예시
        AudioSFX.Instance.PlayOneShot(Manager.Asset.GetAudio("testatk"));

        playerAnimController.OnRangeAtk();
        MakeRangeProjectile();
    }

    //OnRangeAtk에 의해 동작하는 애니메이션에 Event로 동작하는 메서드 입니다.
    public void MakeRangeProjectile()
    {
        // 공격 projectile 생성
        var testProjectile = Manager.ObjectPool.GetGo("PlayerProjectileFrame");
        testProjectile.transform.position = ProjectilePoint.position;
        enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = enemyList[0].transform.position;
        FinalAttackDamage(out testProjectile.GetComponent<PlayerProjectileHandler>().Damage, out testProjectile.GetComponent<PlayerProjectileHandler>().DamageTypeValue);
    }

    private bool IsCritical()
    {
        int chance = UnityEngine.Random.Range(1, 1001);

        if (chance < CritChance.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FinalAttackDamage(out long damage, out DamageType damageTypeValue)
    {
        if (IsCritical())
        {
            damage = (long)((AtkDamage.Value
                + AtkDamage.Value * EquipAttackStat / 100
                + AtkDamage.Value * RetentionAttackEffect / 100) * (1 + CritDamage.GetFloat()) * _damageBuff);
            damageTypeValue = DamageType.Critical;
        }
        else
        {
            damage = (long)((AtkDamage.Value
                + AtkDamage.Value * EquipAttackStat / 100
                + AtkDamage.Value * RetentionAttackEffect / 100)
                * _damageBuff);
            damageTypeValue = DamageType.Normal;
        }
    }

    private float AttakSpeedToTime()
    {
        return 1.0f / AtkSpeed.GetFloat();
    }

    #endregion

    #region Currency Methods

    public bool IsTradeGold(long amount)
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

    public bool IsTradeGems(int amount)
    {
        if (Gems - amount < 0)
        {
            return false;
        }
        else
        {
            Gems -= amount;
            playerView.SetGemsAmout();
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
        Gems += Amount;
        playerView.SetGemsAmout();
    }

    // 데이터 로드 순서 상 SetStage로 스테이지 데이터가 활성화 되는 부분에서 동작하게 함
    public void IdleRewardInit()
    {
        InitIdleGoldReward(out int leftTime);
        StartCoroutine(IdleGoldCoroutine(leftTime));
    }

    public void PopupUIInit(Action action)
    {
        IdleRewardUpdate += action;
    }

    private IEnumerator IdleGoldCoroutine(int leftTime)
    {
        bool isFirst = true;
        int Delay;

        while (true)
        {
            if (isFirst)
            {
                Delay = 60 - leftTime;
                isFirst = false;
            }
            else
            {
                Delay = 60;
            }
            yield return new WaitForSeconds(Delay);
            ToTalIdleGold += Manager.Stage.IdleGoldReward;
            ToTalIdleTime += 1;
            IdleCheckTime = DateTime.Now;
            IdleRewardPopupUpdate();
        }
    }

    public void IdleRewardPopupUpdate()
    {
        IdleRewardUpdate?.Invoke();
    }

    public void IdleRewardReset()
    {
        ToTalIdleGold = 0;
        ToTalIdleTime = 0;
    }

    #endregion

    #region Equipment Methods

    public void EquipmentStatModifier()
    {
        RetentionAttackEffect = 0;
        EquipAttackStat = 0;
        RetentionHPEffect = 0;
        EquipHPStat = 0;

        foreach (var item in Manager.Inventory.UserInventory.UserItemData.Where(itemData => itemData.level > 1 || itemData.hasCount > 0).ToList())
        {
            if (Manager.Inventory.ItemDataDictionary[item.itemID].StatType == "attack")
            {
                RetentionAttackEffect += Manager.Inventory.ItemDataDictionary[item.itemID].RetentionEffect + Manager.Inventory.ItemDataDictionary[item.itemID].ReinforceEffect * item.level;
            }
            else if (Manager.Inventory.ItemDataDictionary[item.itemID].StatType == "hp")
            {
                RetentionHPEffect += Manager.Inventory.ItemDataDictionary[item.itemID].RetentionEffect + Manager.Inventory.ItemDataDictionary[item.itemID].ReinforceEffect * item.level;
            }
        }

        var filteredEquipItem = Manager.Inventory.UserInventory.UserItemData.Where(itemdata => itemdata.equipped == true).ToList();

        foreach (var item in filteredEquipItem)
        {
            if (Manager.Inventory.ItemDataDictionary[item.itemID].StatType == "attack" && item.equipped == true)
            {
                EquipAttackStat += Manager.Inventory.ItemDataDictionary[item.itemID].EquipStat + Manager.Inventory.ItemDataDictionary[item.itemID].ReinforceEquip * item.level;
            }
            else if (Manager.Inventory.ItemDataDictionary[item.itemID].StatType == "hp" && item.equipped == true)
            {
                EquipHPStat += Manager.Inventory.ItemDataDictionary[item.itemID].EquipStat + Manager.Inventory.ItemDataDictionary[item.itemID].ReinforceEquip * item.level;
            }
        }
    }

    #endregion

    #region Time Methods

    public DateTime GetLoginTime()
    {
        return DateTime.ParseExact(profile.Date_Login, "yyyy/MM/dd HH:mm:ss", null);
    }

    public DateTime GetLogoutTime()
    {
        return DateTime.ParseExact(profile.Date_Logout, "yyyy/MM/dd HH:mm:ss", null);
    }

    public void SetBonusCheck(bool isBonusCheck)
    {
        IsBonusCheck = isBonusCheck;
    }

    public void SetIdleCheckTime(DateTime idleCheckTime)
    {
        IdleCheckTime = idleCheckTime;
    }

    public void SetBonusCheckTime(DateTime bonusCheckTime)
    {
        BonusCheckTime = bonusCheckTime;
    }

    #endregion
}
