using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISceneMain : UIScene
{
    #region Player Fields

    private Player player;

    private UIUpgradeStat UpgradeStat_Hp;
    private UIUpgradeStat UpgradeStat_HpRecovery;
    private UIUpgradeStat UpgradeStat_AttackDamage;
    private UIUpgradeStat UpgradeStat_AttackSpeed;
    private UIUpgradeStat UpgradeStat_CriticalChance;
    private UIUpgradeStat UpgradeStat_CriticalDamage;

    private TextMeshProUGUI txt_Gold;
    private TextMeshProUGUI txt_Gems;
    private TextMeshProUGUI txt_Stage;

    #endregion

    #region Fields

    private Button _btnGameSpeedUp;
    private Button _btnOption;
    private Button _btnQuest;

    private Button _btnBoss;
    private Button _btnEquipment;
    private Button _btnShop;

    private TextMeshProUGUI _txtQuestNum;
    private TextMeshProUGUI _txtQuestObjective;
    private TextMeshProUGUI _textQuestReward;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetTexts();
        SetButtons();

        // 데이터 초기화
        player = Manager.Game.Player;
        SetUpgradeStats();
        SetUI();
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        txt_Gold = GetUI<TextMeshProUGUI>("Txt_Gold");
        txt_Gems = GetUI<TextMeshProUGUI>("Txt_Jewel");
        txt_Stage = GetUI<TextMeshProUGUI>("Txt_Stage");

        _txtQuestNum = GetUI<TextMeshProUGUI>("Txt_QuestNumber");
        _txtQuestObjective = GetUI<TextMeshProUGUI>("Txt_QuestObjective");
        _textQuestReward = GetUI<TextMeshProUGUI>("Txt_QuestReward");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _btnGameSpeedUp = SetButtonEvent("Btn_Plain_GameSpeedUP", UIEventType.Click, OnGameSpeedUp);
        _btnOption = SetButtonEvent("Btn_Plain_Option", UIEventType.Click, OnOption);
        _btnQuest = SetButtonEvent("Image_HUD_Quest", UIEventType.Click, OnQuest);

        _btnBoss = SetButtonEvent("Btn_Boss", UIEventType.Click, OnBossStage);
        _btnEquipment = SetButtonEvent("Btn_Equipment", UIEventType.Click, OnEquipment);
        _btnShop = SetButtonEvent("Btn_Shop", UIEventType.Click, OnShop);
    }

    private void SetUpgradeStats()
    {
        SetUI<UIUpgradeStat>();
        UpgradeStat_Hp = GetUI<UIUpgradeStat>("Upgrade_Stat_Hp");
        UpgradeStat_HpRecovery = GetUI<UIUpgradeStat>("Upgrade_Stat_HpRecovery");
        UpgradeStat_AttackDamage = GetUI<UIUpgradeStat>("Upgrade_Stat_AttackDamage");
        UpgradeStat_AttackSpeed = GetUI<UIUpgradeStat>("Upgrade_Stat_AttackSpeed");
        UpgradeStat_CriticalChance = GetUI<UIUpgradeStat>("Upgrade_Stat_CriticalChance");
        UpgradeStat_CriticalDamage = GetUI<UIUpgradeStat>("Upgrade_Stat_CriticalDamage");

        UpgradeStat_Hp.SetUpgradeStat(player, player.Hp, OnHpUp);
        UpgradeStat_HpRecovery.SetUpgradeStat(player, player.HpRecovery, OnHpRecoverUp);
        UpgradeStat_AttackDamage.SetUpgradeStat(player, player.AtkDamage, OnAttackDamageUp);
        UpgradeStat_AttackSpeed.SetUpgradeStat(player, player.AtkSpeed, OnAttackSpeedUp);
        UpgradeStat_CriticalChance.SetUpgradeStat(player, player.CritChance, OnCriticalChanceUp);
        UpgradeStat_CriticalDamage.SetUpgradeStat(player, player.CritDamage, OnCriticalDamageUp);
    }

    private void SetUI()
    {
        // HUD UI
        txt_Gold.text = player.Gold.ToString();
        txt_Gems.text = player.Gems.ToString();
        txt_Stage.text = ($"{Manager.Stage.CurrentStage} - {Manager.Stage.StageProgress}");

        // Stage Button Init
        Manager.Stage.SetRetryBossButton(_btnBoss);

        // Quest
        _txtQuestObjective.text = QuestObjective();
    }

    #endregion

    #region Button Events

    private void OnHpUp(PointerEventData eventData) => UpgradeStat_Hp.UpdateUpgradeStat(player.Hp);
    private void OnHpRecoverUp(PointerEventData eventData) => UpgradeStat_HpRecovery.UpdateUpgradeStat(player.HpRecovery);
    private void OnAttackDamageUp(PointerEventData eventData) => UpgradeStat_AttackDamage.UpdateUpgradeStat(player.AtkDamage);
    private void OnAttackSpeedUp(PointerEventData eventData) => UpgradeStat_AttackSpeed.UpdateUpgradeStat(player.AtkSpeed);
    private void OnCriticalChanceUp(PointerEventData eventData) => UpgradeStat_CriticalChance.UpdateUpgradeStat(player.CritChance);
    private void OnCriticalDamageUp(PointerEventData eventData) => UpgradeStat_CriticalDamage.UpdateUpgradeStat(player.CritDamage);

    private void OnBossStage(PointerEventData eventData)
    {
        Manager.Stage.RetryBossBattle();
    }

    private void OnEquipment(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupEquipment>();
    }

    private void OnShop(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupShopSummon>();
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
        Manager.Quest.CheckQuestCompletion();
        UpdateQuestObjective();
        Debug.Log("퀘스트 버튼"); // 버튼 작동 테스트

        Debug.Log(Manager.Quest.CuurntQuest.questObjective);
    }

    #endregion

    #region Update UI

    public void UpdateGold()
    {
        txt_Gold.text = player.Gold.ToString();
    }

    public void UpdateGems()
    {
        txt_Gems.text = player.Gems.ToString();
    }

    public void UpdateCurrentStage(string currentStage)
    {
        txt_Stage.text = currentStage;
    }

    public void UpdateButtonEnable()
    {

    }

    public void UpdateQuestObjective()
    {
        _txtQuestObjective.text = QuestObjective();
    }

    #endregion

    // 임시. 퀘스트 목표 TEXT 내용 반환
    public string QuestObjective()
    {
        return ($"{Manager.Quest.CuurntQuest.questObjective} {Manager.Quest.CuurntQuest.currentValue} / {Manager.Quest.CuurntQuest.objectValue}");
    }
}
