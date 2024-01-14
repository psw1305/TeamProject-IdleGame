using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISceneMain : UIScene
{
    #region Fields
    private Player _player;

    private Button _btnStatUp_Damage;
    private Button _btnStatUp_HP;
    private Button _btnStat_AttackSpeed;
    private Button _btnStat_RecoverHP;
    private Button _btnStat_CriticalPercent;
    private Button _btnStat_CriticalDamage;
    private Button _btnEquipment;

    private TextMeshProUGUI _txtPayGold_Damage;
    private TextMeshProUGUI _txtStat_Damage;
    private TextMeshProUGUI _txtLv_Damage;

    private TextMeshProUGUI _txtPayGold_HP;
    private TextMeshProUGUI _txtStat_HP;
    private TextMeshProUGUI _txtLv_HP;

    private TextMeshProUGUI _txtPayGold_AttackSpeed;
    private TextMeshProUGUI _txtStat_AttackSpeed;
    private TextMeshProUGUI _txtLv_AttackSpeed;

    private TextMeshProUGUI _txtPayGold_RecoverHP;
    private TextMeshProUGUI _txtStat_RecoverHP;
    private TextMeshProUGUI _txtLv_RecoverHP;

    private TextMeshProUGUI _txtPayGold_CriticalPercent;
    private TextMeshProUGUI _txtStat_CriticalPercent;
    private TextMeshProUGUI _txtLv_CriticalPercent;

    private TextMeshProUGUI _txtPayGold_CriticalDamage;
    private TextMeshProUGUI _txtStat_CriticalDamage;
    private TextMeshProUGUI _txtLv_CriticalDamage;


    private TextMeshProUGUI _txtGold;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        
        // 여기에 플레이어를 가져와서 데이터를 사용해도 됩니다.
        _player = Manager.Game.Player;

        SetTexts();
        SetButtons();
        SetEvents();

        SetStatData();
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        _txtPayGold_Damage = GetUI<TextMeshProUGUI>("Txt_PayGold_Damage");
        _txtStat_Damage = GetUI<TextMeshProUGUI>("Txt_Stat_Damage");
        _txtLv_Damage = GetUI<TextMeshProUGUI>("Txt_Lv_Damage");

        _txtPayGold_HP = GetUI<TextMeshProUGUI>("Txt_PayGold_HP");
        _txtStat_HP = GetUI<TextMeshProUGUI>("Txt_Stat_HP");
        _txtLv_HP = GetUI<TextMeshProUGUI>("Txt_Lv_HP");

        _txtPayGold_AttackSpeed = GetUI<TextMeshProUGUI>("Txt_PayGold_AttackSpeed");
        _txtStat_AttackSpeed = GetUI<TextMeshProUGUI>("Txt_Stat_AttackSpeed");
        _txtLv_AttackSpeed = GetUI<TextMeshProUGUI>("Txt_Lv_AttackSpeed");

        _txtPayGold_RecoverHP = GetUI<TextMeshProUGUI>("Txt_PayGold_RecoverHP");
        _txtStat_RecoverHP = GetUI<TextMeshProUGUI>("Txt_Stat_RecoverHP");
        _txtLv_RecoverHP = GetUI<TextMeshProUGUI>("Txt_Lv_RecoverHP");

        _txtPayGold_CriticalPercent = GetUI<TextMeshProUGUI>("Txt_PayGold_CriticalPercent");
        _txtStat_CriticalPercent = GetUI<TextMeshProUGUI>("Txt_Stat_CriticalPercent");
        _txtLv_CriticalPercent = GetUI<TextMeshProUGUI>("Txt_Lv_CriticalPercent");

        _txtPayGold_CriticalDamage = GetUI<TextMeshProUGUI>("Txt_PayGold_CriticalDamage");
        _txtStat_CriticalDamage = GetUI<TextMeshProUGUI>("Txt_Stat_CriticalDamage");
        _txtLv_CriticalDamage = GetUI<TextMeshProUGUI>("Txt_Lv_CriticalDamage");

        // HUD UI
        _txtGold = GetUI<TextMeshProUGUI>("Txt_Gold");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _btnStatUp_Damage = GetUI<Button>("Btn_StatUp_Damage");
        _btnStatUp_HP = GetUI<Button>("Btn_StatUp_HP");
        _btnStat_AttackSpeed = GetUI<Button>("Btn_StatUp_AttackSpeed");
        _btnStat_RecoverHP = GetUI<Button>("Btn_StatUp_RecoverHP");
        _btnStat_CriticalPercent = GetUI<Button>("Btn_StatUp_CriticalPercent");
        _btnStat_CriticalDamage = GetUI<Button>("Btn_StatUp_CriticalDamage");
        _btnEquipment = GetUI<Button>("Btn_Equipment");
    }

    private void SetEvents()
    {
        _btnStatUp_Damage.gameObject.SetEvent(UIEventType.Click, OnDamageUp);
        _btnStatUp_HP.gameObject.SetEvent(UIEventType.Click, OnHealthUp);
        _btnStat_AttackSpeed.gameObject.SetEvent(UIEventType.Click, OnAttackSpeedUp);
        _btnStat_RecoverHP.gameObject.SetEvent(UIEventType.Click, OnRecoverHPUp);
        _btnStat_CriticalPercent.gameObject.SetEvent(UIEventType.Click, OnCriticalPercentUp);
        _btnStat_CriticalDamage.gameObject.SetEvent(UIEventType.Click, OnCriticalDamageUp);
        _btnEquipment.gameObject.SetEvent(UIEventType.Click, OnEquipment);
    }

    private void SetStatData()
    {
        _txtStat_Damage.text = _player.Damage.ToString();
        _txtLv_Damage.text = _player.DamageInfo.Level.ToString();
        _txtPayGold_Damage.text = _player.DamageInfo.UpgradeCost.ToString();
                        
        _txtStat_HP.text = _player.HP.ToString();
        _txtLv_HP.text = _player.HPInfo.Level.ToString();
        _txtPayGold_HP.text = _player.HPInfo.UpgradeCost.ToString();

        _txtStat_AttackSpeed.text = _player.AttackSpeed.ToString();
        _txtLv_AttackSpeed.text = _player.AttackSpeedInfo.Level.ToString();
        _txtPayGold_AttackSpeed.text = _player.AttackSpeedInfo.UpgradeCost.ToString();

        _txtStat_RecoverHP.text = _player.RecoverHP.ToString();
        _txtLv_RecoverHP.text = _player.RecoverHPInfo.Level.ToString();
        _txtPayGold_RecoverHP.text = _player.RecoverHPInfo.UpgradeCost.ToString();

        _txtStat_CriticalPercent.text = _player.CriticalPercent.ToString();
        _txtLv_CriticalPercent.text = _player.CriticalPercentInfo.Level.ToString();
        _txtPayGold_CriticalPercent.text = _player.CriticalPercentInfo.UpgradeCost.ToString();

        _txtStat_CriticalDamage.text = _player.CriticalDamage.ToString();
        _txtLv_CriticalDamage.text = _player.CriticalDamageInfo.Level.ToString();
        _txtPayGold_CriticalDamage.text = _player.CriticalDamageInfo.UpgradeCost.ToString();

        //HUD UI
        _txtGold.text = _player.Gold.ToString();
    }

    #endregion

    #region Button Events

    private void OnDamageUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.DamageInfo.UpgradeCost)
        {
            Debug.Log("Damage_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("DamageUp + 10");
            //플레이어 공격력 증가
            _player.DamageUp(10);
            _txtStat_Damage.text = _player.Damage.ToString();


            UseGold(_player.DamageInfo.UpgradeCost);
            _player.DamageInfo.SetModifier(1, 50);
            _txtLv_Damage.text = _player.DamageInfo.Level.ToString();
            _txtPayGold_Damage.text = _player.DamageInfo.UpgradeCost.ToString();
        }
    }

    private void OnHealthUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.HPInfo.UpgradeCost)
        {
            Debug.Log("HP_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("HealthUp + 10");
            //플레이어 체력 증가
            _player.HpUp(10);
            _txtStat_HP.text = _player.HP.ToString();


            UseGold(_player.HPInfo.UpgradeCost);
            _player.HPInfo.SetModifier(1, 40);
            _txtLv_HP.text = _player.HPInfo.Level.ToString();
            _txtPayGold_HP.text = _player.HPInfo.UpgradeCost.ToString();
        }
    }

    private void OnAttackSpeedUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.AttackSpeedInfo.UpgradeCost)
        {
            Debug.Log("AttackSpeed_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("AttackSpeed + 0.01f");
            //플레이어 공격 속도 증가
            _player.AttackSpeedUp(0.01f);
            _txtStat_AttackSpeed.text = _player.AttackSpeed.ToString();

            UseGold(_player.AttackSpeedInfo.UpgradeCost);
            _player.AttackSpeedInfo.SetModifier(1, 60);
            _txtLv_AttackSpeed.text = _player.AttackSpeedInfo.Level.ToString();
            _txtPayGold_AttackSpeed.text = _player.AttackSpeedInfo.UpgradeCost.ToString();
        }
    }

    private void OnRecoverHPUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.RecoverHPInfo.UpgradeCost)
        {
            Debug.Log("RecoverHP_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("RecoverHP + 10");
            //플레이어 회복 속도 증가
            _player.RecoverHPUp(10);
            _txtStat_RecoverHP.text = _player.RecoverHP.ToString();

            UseGold(_player.RecoverHPInfo.UpgradeCost);
            _player.RecoverHPInfo.SetModifier(1, 40);
            _txtLv_RecoverHP.text = _player.RecoverHPInfo.Level.ToString();
            _txtPayGold_RecoverHP.text = _player.RecoverHPInfo.UpgradeCost.ToString();
        }
    }

    private void OnCriticalPercentUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.CriticalPercentInfo.UpgradeCost)
        {
            Debug.Log("CriticalPercent_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("CriticalPercent + 10");
            //플레이어 회복 속도 증가
            _player.CriticalPercentUp(0.1f);
            _txtStat_CriticalPercent.text = _player.CriticalPercent.ToString();

            UseGold(_player.CriticalPercentInfo.UpgradeCost);
            _player.CriticalPercentInfo.SetModifier(1, 40);
            _txtLv_CriticalPercent.text = _player.CriticalPercentInfo.Level.ToString();
            _txtPayGold_CriticalPercent.text = _player.CriticalPercentInfo.UpgradeCost.ToString();
        }
    }
    
    private void OnCriticalDamageUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.CriticalDamageInfo.UpgradeCost)
        {
            Debug.Log("CriticalDamage_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("CriticalDamage + 10");
            //플레이어 회복 속도 증가
            _player.CriticalDamageUp(0.1f);
            _txtStat_CriticalDamage.text = _player.CriticalDamage.ToString();

            UseGold(_player.CriticalDamageInfo.UpgradeCost);
            _player.CriticalDamageInfo.SetModifier(1, 40);
            _txtLv_CriticalDamage.text = _player.CriticalDamageInfo.Level.ToString();
            _txtPayGold_CriticalDamage.text = _player.CriticalDamageInfo.UpgradeCost.ToString();
        }
    }

    private void OnEquipment(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupEquipment>();
    }

    #endregion

    #region Gold

    public void GetRewards()
    {
        _txtGold.text = _player.Gold.ToString();
    }

    private void UseGold(long amount)
    {
        _player.UseGold(amount);
        GetRewards();
    }

    #endregion
}
