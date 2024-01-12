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

    private TextMeshProUGUI _txtPayGoldDamage;
    private TextMeshProUGUI _txtStatDamage;
    private TextMeshProUGUI _txtLvDamage;

    private TextMeshProUGUI _txtPayGoldHp;
    private TextMeshProUGUI _txtStatHp;
    private TextMeshProUGUI _txtLvHp;

    private TextMeshProUGUI _txtsPayGoldAttackSpeed;
    private TextMeshProUGUI _txtStatAttackSpeed;
    private TextMeshProUGUI _txtLvAttackSpeed;

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

        _txtPayGoldHp = GetUI<TextMeshProUGUI>("Txt_PayGold_Hp");
        _txtStatHp = GetUI<TextMeshProUGUI>("Txt_Stat_Hp");

        _txtsPayGoldAttackSpeed = GetUI<TextMeshProUGUI>("Txt_PayGold_AttackSpeed");
        _txtStatAttackSpeed = GetUI<TextMeshProUGUI>("Txt_Stat_AttackSpeed");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _btnStatUpDamage = GetUI<Button>("Btn_StatUp_Damage");
        _btnStatUpHp = GetUI<Button>("Btn_StatUp_Hp");
        _btnStatAttackSpeed = GetUI<Button>("Btn_StatUp_AttackSpeed");
    }

    private void SetEvents()
    {
        _btnStatUpDamage.gameObject.SetEvent(UIEventType.Click, OnDamageUp);
        _btnStatUpHp.gameObject.SetEvent(UIEventType.Click, OnHealthUp);
        _btnStatAttackSpeed.gameObject.SetEvent(UIEventType.Click, OnAttackSpeedUp);
    }

    private void SetStatData()
    {
        _txtStatDamage.text = _player.Damage.ToString();
        _txtStatHp.text = _player.Hp.ToString();
        _txtStatAttackSpeed.text = _player.AttackSpeed.ToString();
    }

    #endregion

    #region Button Events

    private void OnDamageUp(PointerEventData eventData)
    {
        Debug.Log("DamageUp + 10");
        //플레이어 공격력 증가
        _player.DamageUp(10);
        _txtStatDamage.text = _player.Damage.ToString();
    }

    private void OnHealthUp(PointerEventData eventData)
    {
        Debug.Log("HealthUp + 10 ");
        //플레이어 체력 증가
        _player.HpUp(10);        
        _txtStatHp.text = _player.Hp.ToString();
    }

    private void OnAttackSpeedUp(PointerEventData eventData)
    {
        Debug.Log("AttackSpeed");
        //플레이어 공격 속도 증가
        _player.AttackSpeedUp(0.01d);
        _txtStatAttackSpeed.text = _player.AttackSpeed.ToString();
    }

    #endregion
}
