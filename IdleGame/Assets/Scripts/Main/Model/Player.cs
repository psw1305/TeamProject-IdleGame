using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour, IDamageable
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;
    [SerializeField] private Image HPGauge;

    #endregion

    #region Fields

    public List<BaseEnemy> _enemyList;
    private Rigidbody2D _playerRigidbody;
    private GameObject _enemy;

    #endregion

    #region UpgradeInfoStrut

    public struct UpgradeInfo
    {
        public int Level;
        public long UpgradeCost;

        public UpgradeInfo(int Lv, long Cost)
        {
            this.Level = Lv;
            this.UpgradeCost = Cost;
        }

        public void SetModifier(int Lv, long Cost)
        {
            this.Level += Lv;
            this.UpgradeCost += Cost;
        }
    }

    #endregion

    #region Properties

    public long Damage { get; private set; }
    public long HP { get; private set; }
    private long _currentHP;
    public long RecoverHP { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CriticalPercent { get; private set; }
    public float CriticalDamage { get; private set; }
    public float Range { get; private set; }
    public int Speed { get; private set; }
    public long Gold { get; private set; }
    public int Jewel { get; private set; }
    public float equipStat { get; private set; }
    public float retentionEffect {  get; private set; }

    private Coroutine _attackCoroutine;
    public UpgradeInfo DamageInfo;
    public UpgradeInfo HPInfo;
    public UpgradeInfo AttackSpeedInfo;
    public UpgradeInfo RecoverHPInfo;
    public UpgradeInfo CriticalPercentInfo;
    public UpgradeInfo CriticalDamageInfo;

    #endregion

    #region Init

    private void Start()
    {
        HP = 1000;
        RecoverHP = 30;

        Damage = 10;
        AttackSpeed = 0.50f;
        CriticalPercent = 50.00f;
        CriticalDamage = 1000;

        Range = 5;
        Speed = 100;

        DamageInfo = new UpgradeInfo(1, 90);
        HPInfo = new UpgradeInfo(1, 50);
        AttackSpeedInfo = new UpgradeInfo(1, 10);
        RecoverHPInfo = new UpgradeInfo(1, 50);
        CriticalPercentInfo = new UpgradeInfo(1, 100);
        CriticalDamageInfo = new UpgradeInfo(1, 100);

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _enemyList = Manager.Stage.GetEnemyList();

        PlayerHPReset();
        Manager.Inventory.InitItem();
        EquipmentStatModifier();
        StartCoroutine(RecoverHealthPoint());
    }

    public void PlayerHPReset()
    {
        _currentHP = HP;
    }

    #endregion

    #region StatModifier

    public void DamageUp(long modifier)
    {
        Damage += modifier;
    }

    public void HpUp(long modifier)
    {
        HP += modifier;
    }

    public void AttackSpeedUp(float modifier)
    {
        AttackSpeed += modifier;
        AttackSpeed = Mathf.Round((float)AttackSpeed * 100) / 100;
    }

    public void RecoverHPUp(long modifier)
    {
        RecoverHP += modifier;
    }

    public void CriticalPercentUp(float modifier)
    {
        CriticalPercent += modifier;
    }
    
    public void CriticalDamageUp(float modifier)
    {
        CriticalDamage += modifier;
    }

    public void UseGold(long amount)
    {        
        Gold -= amount;
    }

    public void EquipmentStatModifier()
    {
        retentionEffect = 0;
        equipStat = 0;
        foreach (var item in Manager.Inventory.itemDataBase.ItemDB)
        {
            retentionEffect += item.retentionEffect + item.reinforceEffect * item.level;
        }
        var filteredEquipItem = Manager.Inventory.itemDataBase.ItemDB.Where(itemdata => itemdata.equipped == true).ToList();
        foreach (var item in filteredEquipItem)
        {
            equipStat += item.equipStat + item.reinforceEquip *item.level;
        }
        Debug.Log($"retentionEffect : {retentionEffect}");
        Debug.Log($"equipStat : {equipStat}");
    }
    #endregion

    private void FixedUpdate()
    {
        // TODO : 범위 내에 도달 시 공격
        //_playerRigidbody.velocity = Vector2.zero;
        //Attack(); // TODO : 공격이 Update로 계속 실행되서 projectile이 반복 생성되어 수정 필요함
        if (_attackCoroutine == null && _enemyList.Count > 0)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    #region State
    public void Idle()
    {
        _playerRigidbody.velocity = Vector2.right * Speed * Time.deltaTime;
    }

    #endregion

    #region Attack Method

    public void Attack()
    {
        // 공격 projectile 생성
        var testProjectile = Manager.Resource.InstantiatePrefab("PlayerProjectileFrame", ProjectilePoint);

        _enemyList[0].gameObject.layer = LayerMask.NameToLayer("TargetEnemy");

        testProjectile.GetComponent<PlayerProjectileHandler>().TargetPosition = _enemyList[0].transform.position;
        if (Random.Range(1, 10001) < CriticalPercent * 100)
        {
            testProjectile.GetComponent<PlayerProjectileHandler>().Damage = (long)(Damage + (Damage * CriticalDamage)*(1 + equipStat / 100) *(1 + retentionEffect / 100)) ;
            Debug.Log("크리티컬 : " + NumUnit.ConvertToString(testProjectile.GetComponent<PlayerProjectileHandler>().Damage));
        }
        else
        {
            testProjectile.GetComponent<PlayerProjectileHandler>().Damage = (long)(Damage * (1 + equipStat / 100) * (1 + retentionEffect / 100));
            Debug.Log("일반 : " + testProjectile.GetComponent<PlayerProjectileHandler>().Damage);
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / AttackSpeed);
            //대상이 없으면 break
            if (_enemyList.Count == 0)
            {
                _attackCoroutine = null;
                break;
            }
            //
            if (Vector2.Distance(transform.position, _enemyList[0].transform.position) <= Range)
            {
                Attack();
            }
        }
    }

    #endregion

    #region HP, DamageMethod
    IEnumerator RecoverHealthPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _currentHP = (long)Mathf.Clamp(_currentHP + RecoverHP, 0, HP);
            SetHPUI();
        }
    }

    private void SetHPUI()
    {
        float FillAmount = Mathf.Clamp((float)_currentHP / HP, 0, 1);
        HPGauge.fillAmount = FillAmount;
    }

    public void TakeDamage(long Damage)
    {
        AmountDamage(Damage);
        FloatingDamage(new Vector3(0, 0.25f, 0), Damage);
        SetHPUI();
    }

    public void FloatingDamage(Vector3 position, long Damage) 
    {
        GameObject DamageHUD = Manager.Resource.InstantiatePrefab("Canvas_FloatingDamage");
        DamageHUD.transform.position = this.gameObject.transform.position + position;
        DamageHUD.GetComponentInChildren<UIFloatingText>().SetDamage(Damage);
    }

    private void AmountDamage(long Damage)
    {
        if (_currentHP - Damage <= 0)
        {
            _currentHP = 0;
            Die();
        }
        else
        {
            _currentHP -= Damage;
        }
    }
    private void Die()
    {
        //이전 스테이지로
        Manager.Stage.StageFailed();
        PlayerHPReset();
        Debug.Log("Player Die...");
    }

    #endregion

    #region RewardMethod

    public void AmountGold(long Amount)
    {
        Gold = (long)Mathf.Clamp(Gold + Amount, 0, 1_000_000_000_000_000_000);
        
        UISceneMain uISceneTest = Manager.UI.CurrentScene as UISceneMain; // 변수화 
        uISceneTest.DisplayGold();
    }

    #endregion
}
