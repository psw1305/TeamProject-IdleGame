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
    private TextMeshProUGUI txt_Difficult;
    private TextMeshProUGUI txt_Stage;

    private Image Image_WaveLoop;
    private Image Image_LevelGauge;

    #endregion

    #region Fields

    private Button _btnGameSpeedUp;
    private Button _btnOption;
    private Button _btnQuest;

    private Button _btnBoss;
    private Button _btnEquipment;
    private Button _btnShop;
    private Button _btnSave;

    private TextMeshProUGUI _txtQuestNum;
    private TextMeshProUGUI _txtQuestObjective;
    private TextMeshProUGUI _textQuestReward;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetImages();
        SetTexts();
        SetButtons();

        // 데이터 초기화
        player = Manager.Game.Player;

        SetUpgradeStats();
        SetUI();
    }

    private void SetImages()
    {
        SetUI<Image>();
        Image_WaveLoop = GetUI<Image>("LoopImage");
        Image_LevelGauge = GetUI<Image>("ProgressGauge");
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        txt_Gold = GetUI<TextMeshProUGUI>("Txt_Gold");
        txt_Gems = GetUI<TextMeshProUGUI>("Txt_Jewel");
        txt_Difficult = GetUI<TextMeshProUGUI>("Txt_Difficult");
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
        _btnQuest = SetButtonEvent("Image_HUD_Quest", UIEventType.Click, OnQuestComplete);

        _btnBoss = SetButtonEvent("Btn_Boss", UIEventType.Click, OnBossStage);
        _btnEquipment = SetButtonEvent("Btn_Equipment", UIEventType.Click, OnEquipment);
        _btnShop = SetButtonEvent("Btn_Shop", UIEventType.Click, OnShop);
        _btnSave = SetButtonEvent("Btn_Save", UIEventType.Click, OnSave);
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
        // Update Top UI
        UpdateGold();
        UpdateGems();
        UpdateCurrentStage();

        // Quest
        UpdateQuestNum();
        _txtQuestObjective.text = QuestObjective();

        // Stage Button Off
        _btnBoss.gameObject.SetActive(false);
        Image_WaveLoop.gameObject.SetActive(false);

        if (Manager.Stage.WaveLoop)
        {
            RetryBossButtonToggle();
            WaveLoopImageToggle();
            StageLevelGaugeToggle(false);
        }
        UpdateStageLevel(Manager.Stage.StageLevel);
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

    private void OnSave(PointerEventData eventData)
    {
        Manager.Data.SaveData();
    }

    private void OnGameSpeedUp(PointerEventData eventData)
    {
        Debug.Log("게임 스피드 업"); // 버튼 작동 테스트
    }

    private void OnOption(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupOptionDropdownPanel>("Option_DropdownPanel");
    }
    
    private void OnQuestComplete(PointerEventData eventData)
    {
        if (Manager.Quest.IsQuestComplete())
        {
            UpdateQuestNum();
            UpdateQuestObjective();
        }
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

    public void UpdateCurrentStage()
    {
        txt_Difficult.text = ($"{Manager.Stage.DifficultyStr}");
        txt_Stage.text = ($"{Manager.Stage.ChapterStr}");
    }

    public void UpdateButtonEnable()
    {

    }

    public void UpdateQuestNum()
    {
        _txtQuestNum.text = ($"Quest No.{Manager.Quest.QuestNum + 1}");
    }

    public void UpdateQuestObjective()
    {
        _txtQuestObjective.text = QuestObjective();
    }

    public void UpdateStageLevel(int level)
    {
        Image_LevelGauge.fillAmount = level / 4.0f;
    }

    public void RetryBossButtonToggle()
    {
        _btnBoss.gameObject.SetActive(!_btnBoss.IsActive());
    }

    public void WaveLoopImageToggle()
    {
        Image_WaveLoop.gameObject.SetActive(!Image_WaveLoop.IsActive());
    }

    public void StageLevelGaugeToggle()
    {
        var gauge = Image_LevelGauge.transform.parent.gameObject;
        gauge.SetActive(!gauge.activeSelf);
    }

    public void StageLevelGaugeToggle(bool active)
    {
        var gauge = Image_LevelGauge.transform.parent.gameObject;
        gauge.SetActive(active);
    }

    #endregion

    // 임시. 퀘스트 목표 TEXT 내용 반환
    public string QuestObjective()
    {
        if(Manager.Quest.CurrentQuest.questType == QuestType.StageClear)        
            return ($"{Manager.Quest.CurrentQuest.questObjective} {Manager.Quest.CurrentQuest.objectiveValue} - 0");

        else
            return ($"{Manager.Quest.CurrentQuest.questObjective} {Manager.Quest.CurrentQuest.currentValue} / {Manager.Quest.CurrentQuest.objectiveValue}");
    }
}
