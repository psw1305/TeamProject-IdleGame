using UnityEngine;
using UnityEngine.Events;
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

    private Button _btnGameSpeedUp;
    private Button _btnOption;
    private Button _btnQuest;

    private Button _btnBoss;
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
    private TextMeshProUGUI _txtJewel;
    private TextMeshProUGUI _txtStage;

    private TextMeshProUGUI _txtQuestNum;
    private TextMeshProUGUI _txtQuestObjective;
    private TextMeshProUGUI _textQuestReward;

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
        _txtJewel = GetUI<TextMeshProUGUI>("Txt_Jewel");
        _txtStage = GetUI<TextMeshProUGUI>("Txt_Stage");

        _txtQuestNum = GetUI<TextMeshProUGUI>("Txt_QuestNumber");
        _txtQuestObjective = GetUI<TextMeshProUGUI>("Txt_QuestObjective");
        _textQuestReward = GetUI<TextMeshProUGUI>("Txt_QuestReward");
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

        _btnGameSpeedUp = GetUI<Button>("Btn_Plain_GameSpeedUP");
        _btnOption = GetUI<Button>("Btn_Plain_Option");
        _btnQuest = GetUI<Button>("Image_HUD_Quest");

        _btnBoss = GetUI<Button>("Btn_Boss");
        _btnEquipment = GetUI<Button>("Btn_Equipment");
    }

    private void SetEvents()
    {
        _btnStatUp_Damage.gameObject.SetEvent(UIEventType.Click, OnAttackDamageUp);
        _btnStatUp_HP.gameObject.SetEvent(UIEventType.Click, OnHpUp);
        _btnStat_AttackSpeed.gameObject.SetEvent(UIEventType.Click, OnAttackSpeedUp);
        _btnStat_RecoverHP.gameObject.SetEvent(UIEventType.Click, OnHpRecoverUp);
        _btnStat_CriticalPercent.gameObject.SetEvent(UIEventType.Click, OnCriticalChanceUp);
        _btnStat_CriticalDamage.gameObject.SetEvent(UIEventType.Click, OnCriticalDamageUp);

        _btnGameSpeedUp.gameObject.SetEvent(UIEventType.Click, OnGameSpeedUp);
        _btnOption.gameObject.SetEvent(UIEventType.Click, OnOption);
        _btnQuest.gameObject.SetEvent(UIEventType.Click, OnQuest);

        _btnBoss.gameObject.SetEvent(UIEventType.Click, OnBossStage);
        _btnEquipment.gameObject.SetEvent(UIEventType.Click, OnEquipment);
    }

    private void SetStatData()
    {
        SetStatText(_txtStat_Damage, _txtLv_Damage, _txtPayGold_Damage, _player.AttackDamage, _player.UpgradeAttackDamage);
        SetStatText(_txtStat_HP, _txtLv_HP, _txtPayGold_HP, _player.Hp, _player.UpgradeHp);
        SetStatText(_txtStat_AttackSpeed, _txtLv_AttackSpeed, _txtPayGold_AttackSpeed, _player.AttackSpeed, _player.UpgradeAttackSpeed);
        SetStatText(_txtStat_RecoverHP, _txtLv_RecoverHP, _txtPayGold_RecoverHP, _player.HpRecovery, _player.UpgradeHpRecovery);
        SetStatText(_txtStat_CriticalPercent, _txtLv_CriticalPercent, _txtPayGold_CriticalPercent, _player.CriticalChance, _player.UpgradeCriticalChance);
        SetStatText(_txtStat_CriticalDamage, _txtLv_CriticalDamage, _txtPayGold_CriticalDamage, _player.CriticalDamage, _player.UpgradeCriticalDamage);

        // HUD UI
        _txtGold.text = _player.Gold.ToString();
        _txtJewel.text = _player.Gems.ToString();
        _txtStage.text = ($"{Manager.Stage.CurrentStage} - {Manager.Stage.StageProgress}");

        // Stage Button Init
        Manager.Stage.SetRetryBossButton(_btnBoss);
    }

    private void SetStatText(TextMeshProUGUI statText, TextMeshProUGUI levelText, TextMeshProUGUI costText, float statValue, UpgradeInfo upgradeInfo)
    {
        statText.text = statValue.ToString();
        levelText.text = $"Lv {upgradeInfo.Level}";
        costText.text = upgradeInfo.UpgradeCost.ToString();
    }

    #endregion

    #region Button Events

    private void OnHpUp(PointerEventData eventData) => UpgradeStat(_player.UpgradeHp, _player.Hp, _txtStat_HP, _txtLv_HP, _txtPayGold_HP, () => _player.HealthUp(10));
    private void OnHpRecoverUp(PointerEventData eventData) => UpgradeStat(_player.UpgradeHpRecovery, _player.HpRecovery, _txtStat_RecoverHP, _txtLv_RecoverHP, _txtPayGold_RecoverHP, () => _player.HealthRecoveryUp(10));
    private void OnAttackDamageUp(PointerEventData eventData) => UpgradeStat(_player.UpgradeAttackDamage, _player.AttackDamage, _txtStat_Damage, _txtLv_Damage, _txtPayGold_Damage, () => _player.AttackDamageUp(10));
    private void OnAttackSpeedUp(PointerEventData eventData) => UpgradeStat(_player.UpgradeAttackSpeed, _player.AttackSpeed, _txtStat_AttackSpeed, _txtLv_AttackSpeed, _txtPayGold_AttackSpeed, () => _player.AttackSpeedUp(0.01f));
    private void OnCriticalChanceUp(PointerEventData eventData) => UpgradeStat(_player.UpgradeCriticalChance, _player.CriticalChance, _txtStat_CriticalPercent, _txtLv_CriticalPercent, _txtPayGold_CriticalPercent, () => _player.CriticalChanceUp(0.1f));
    private void OnCriticalDamageUp(PointerEventData eventData) => UpgradeStat(_player.UpgradeCriticalDamage, _player.CriticalDamage, _txtStat_CriticalDamage, _txtLv_CriticalDamage, _txtPayGold_CriticalDamage, () => _player.CriticalDamageUp(0.1f));

    private void UpgradeStat(UpgradeInfo upgradeInfo, float statValue, TextMeshProUGUI statText, TextMeshProUGUI levelText, TextMeshProUGUI costText, UnityAction upgrade)
    {
        if (_player.Gold < upgradeInfo.UpgradeCost) return;

        UseGold(upgradeInfo.UpgradeCost);
        upgradeInfo.SetModifier(1, 40);

        statText.text = statValue.ToString();
        levelText.text = $"Lv {upgradeInfo.Level}";
        costText.text = upgradeInfo.UpgradeCost.ToString();

        upgrade?.Invoke();
    }

    private void OnBossStage(PointerEventData eventData)
    {
        Manager.Stage.RetryBossBattle();
    }

    private void OnEquipment(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupEquipment>();
    }

    private void OnGameSpeedUp(PointerEventData eventData)
    {
        Debug.Log("게임 스피드 업"); // 버튼 작동 테스트
    }

    private void OnOption(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupOptionDropdownPanel>("Option_DropdownPanel");
    }
    
    private void OnQuest(PointerEventData eventData)
    {
        Debug.Log("퀘스트 버튼"); // 버튼 작동 테스트
    }

    #endregion

    #region Currency

    public void DisplayGold()
    {
        _txtGold.text = _player.Gold.ToString();
    }

    public void DisplayJewel()
    {
        _txtJewel.text = _player.Gems.ToString();
    }

    private void UseGold(long amount)
    {
        _player.UsedGold(amount);
        DisplayGold();
    }

    #endregion

    public void DisplayCurrentStage(string currentStage)
    {
        _txtStage.text = currentStage;
    }
}
