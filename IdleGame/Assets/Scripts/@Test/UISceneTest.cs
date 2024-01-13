using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISceneTest : UIScene
{
    #region Fields
    private Player _player;

    private Button _btnStatUpDamage;
    private Button _btnStatUpHp;
    private Button _btnStatAttackSpeed;
    private Button _btnStatRecoverHP;

    private TextMeshProUGUI _txtPayGoldDamage;
    private TextMeshProUGUI _txtStatDamage;
    private TextMeshProUGUI _txtLvDamage;

    private TextMeshProUGUI _txtPayGoldHp;
    private TextMeshProUGUI _txtStatHp;
    private TextMeshProUGUI _txtLvHp;

    private TextMeshProUGUI _txtPayGoldAttackSpeed;
    private TextMeshProUGUI _txtStatAttackSpeed;
    private TextMeshProUGUI _txtLvAttackSpeed;

    private TextMeshProUGUI _txtPayGoldRecoverHP;
    private TextMeshProUGUI _txtStatRecoverHP;
    private TextMeshProUGUI _txtLvRecoverHP;

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
        _txtPayGoldDamage = GetUI<TextMeshProUGUI>("Txt_PayGold_Damage");
        _txtStatDamage = GetUI<TextMeshProUGUI>("Txt_Stat_Damage");
        _txtLvDamage = GetUI<TextMeshProUGUI>("Txt_Lv_Damage");

        _txtPayGoldHp = GetUI<TextMeshProUGUI>("Txt_PayGold_Hp");
        _txtStatHp = GetUI<TextMeshProUGUI>("Txt_Stat_Hp");
        _txtLvHp = GetUI<TextMeshProUGUI>("Txt_Lv_Hp");

        _txtPayGoldAttackSpeed = GetUI<TextMeshProUGUI>("Txt_PayGold_AttackSpeed");
        _txtStatAttackSpeed = GetUI<TextMeshProUGUI>("Txt_Stat_AttackSpeed");
        _txtLvAttackSpeed = GetUI<TextMeshProUGUI>("Txt_Lv_AttackSpeed");

        _txtPayGoldRecoverHP = GetUI<TextMeshProUGUI>("Txt_PayGold_RecoverHP");
        _txtStatRecoverHP = GetUI<TextMeshProUGUI>("Txt_Stat_RecoverHP");
        _txtLvRecoverHP = GetUI<TextMeshProUGUI>("Txt_Lv_RecoverHP");

        // HUD UI
        _txtGold = GetUI<TextMeshProUGUI>("Txt_Gold");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _btnStatUpDamage = GetUI<Button>("Btn_StatUp_Damage");
        _btnStatUpHp = GetUI<Button>("Btn_StatUp_Hp");
        _btnStatAttackSpeed = GetUI<Button>("Btn_StatUp_AttackSpeed");
        _btnStatRecoverHP = GetUI<Button>("Btn_StatUp_RecoverHP");
    }

    private void SetEvents()
    {
        _btnStatUpDamage.gameObject.SetEvent(UIEventType.Click, OnDamageUp);
        _btnStatUpHp.gameObject.SetEvent(UIEventType.Click, OnHealthUp);
        _btnStatAttackSpeed.gameObject.SetEvent(UIEventType.Click, OnAttackSpeedUp);
        _btnStatRecoverHP.gameObject.SetEvent(UIEventType.Click, OnRecoverHPUp);
    }

    private void SetStatData()
    {
        _txtStatDamage.text = _player.Damage.ToString();
        _txtLvDamage.text = _player.DamageInfo.Level.ToString();
        _txtPayGoldDamage.text = _player.DamageInfo.UpgradCost.ToString();
                        
        _txtStatHp.text = _player.Hp.ToString();
        _txtLvHp.text = _player.HPInfo.Level.ToString();
        _txtPayGoldHp.text = _player.HPInfo.UpgradCost.ToString();

        _txtStatAttackSpeed.text = _player.AttackSpeed.ToString();
        _txtLvAttackSpeed.text = _player.AttackSpeedInfo.Level.ToString();
        _txtPayGoldAttackSpeed.text = _player.AttackSpeedInfo.UpgradCost.ToString();

        _txtStatRecoverHP.text = _player.RecoverHP.ToString();
        _txtLvRecoverHP.text = _player.RecoverHPInfo.Level.ToString();
        _txtPayGoldRecoverHP.text = _player.RecoverHPInfo.UpgradCost.ToString();

        //HUD UI
        _txtGold.text = _player.Gold.ToString();
    }

    #endregion

    #region Button Events

    private void OnDamageUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.DamageInfo.UpgradCost)
        {
            Debug.Log("Damage_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("DamageUp + 10");
            //플레이어 공격력 증가
            _player.DamageUp(10);
            _txtStatDamage.text = _player.Damage.ToString();


            UseGold(_player.DamageInfo.UpgradCost);
            _player.DamageInfo.SetModifier(1, 50);
            _txtLvDamage.text = _player.DamageInfo.Level.ToString();
            _txtPayGoldDamage.text = _player.DamageInfo.UpgradCost.ToString();
        }
    }

    private void OnHealthUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.HPInfo.UpgradCost)
        {
            Debug.Log("HP_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("HealthUp + 10 ");
            //플레이어 체력 증가
            _player.HpUp(10);
            _txtStatHp.text = _player.Hp.ToString();


            UseGold(_player.HPInfo.UpgradCost);
            _player.HPInfo.SetModifier(1, 40);
            _txtLvHp.text = _player.HPInfo.Level.ToString();
            _txtPayGoldHp.text = _player.HPInfo.UpgradCost.ToString();
        }
    }

    private void OnAttackSpeedUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.AttackSpeedInfo.UpgradCost)
        {
            Debug.Log("AttackSpeed_돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("AttackSpeed");
            //플레이어 공격 속도 증가
            _player.AttackSpeedUp(0.01f);
            _txtStatAttackSpeed.text = _player.AttackSpeed.ToString();

            UseGold(_player.AttackSpeedInfo.UpgradCost);
            _player.AttackSpeedInfo.SetModifier(1, 60);
            _txtLvAttackSpeed.text = _player.AttackSpeedInfo.Level.ToString();
            _txtPayGoldAttackSpeed.text = _player.AttackSpeedInfo.UpgradCost.ToString();
        }
    }

    private void OnRecoverHPUp(PointerEventData eventData)
    {
        if (_player.Gold < _player.RecoverHPInfo.UpgradCost)
        {
            Debug.Log("돈이 모자랍니다.");
        }
        else
        {
            Debug.Log("RecoverHP");
            //플레이어 회복 속도 증가
            _player.RecoverHPUp(10);
            _txtStatRecoverHP.text = _player.RecoverHP.ToString();

            UseGold(_player.RecoverHPInfo.UpgradCost);
            _player.RecoverHPInfo.SetModifier(1, 40);
            _txtLvRecoverHP.text = _player.RecoverHPInfo.Level.ToString();
            _txtPayGoldRecoverHP.text = _player.RecoverHPInfo.UpgradCost.ToString();
        }
    }

    #endregion

    public void GetRewards()
    {
        _txtGold.text = _player.Gold.ToString();
    }

    private void UseGold(long amount)
    {
        _player.UseGold(amount);
        GetRewards();
    }
}
