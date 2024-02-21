using System;
using System.Collections;
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

    private TextMeshProUGUI txt_Stage;
    private TextMeshProUGUI txt_PlayerPower;

    private Image Image_WaveLoop;
    private Image Image_LevelGauge;
    private Image image_1xSpeed;
    private Image image_2xSpeed;

    #endregion

    #region Fields

    private Button _btnBoss;
    private Button _btnIdleRewards;

    private TextMeshProUGUI _txtQuestNum;
    private TextMeshProUGUI _txtQuestObjective;
    private TextMeshProUGUI _textIdleNotice;

    private bool isSpeedUp = false;

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

        IdleRewardsTimeCheck();
    }

    private void SetImages()
    {
        SetUI<Image>();
        Image_WaveLoop = GetUI<Image>("LoopImage");
        Image_LevelGauge = GetUI<Image>("ProgressGauge");
        image_1xSpeed = GetUI<Image>("Image_1xSpeed");
        image_2xSpeed = GetUI<Image>("Image_2xSpeed");
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();

        txt_Stage = GetUI<TextMeshProUGUI>("Txt_Stage");
        txt_PlayerPower = GetUI<TextMeshProUGUI>("Txt_PlayerPower");

        _txtQuestNum = GetUI<TextMeshProUGUI>("Txt_QuestNumber");
        _txtQuestObjective = GetUI<TextMeshProUGUI>("Txt_QuestObjective");
        _textIdleNotice = GetUI<TextMeshProUGUI>("Text_IdleRewards_Notice");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        SetButtonEvent("Btn_Plain_GameSpeed", UIEventType.Click, OnGameSpeed);
        SetButtonEvent("Btn_Plain_Option", UIEventType.Click, OnOption);
        SetButtonEvent("Btn_Quest", UIEventType.Click, OnQuestComplete);

        _btnBoss = SetButtonEvent("Btn_Boss", UIEventType.Click, OnBossStage);
        _btnIdleRewards = SetButtonEvent("Btn_IdleRewards", UIEventType.Click, OnIdleRewards);
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

        UpgradeStat_Hp.SetUpgradeStat(player.Hp);
        UpgradeStat_HpRecovery.SetUpgradeStat(player.HpRecovery);
        UpgradeStat_AttackDamage.SetUpgradeStat(player.AtkDamage);
        UpgradeStat_AttackSpeed.SetUpgradeStat(player.AtkSpeed);
        UpgradeStat_CriticalChance.SetUpgradeStat(player.CritChance);
        UpgradeStat_CriticalDamage.SetUpgradeStat(player.CritDamage);
    }

    private void SetUI()
    {
        // Update Top UI
        UpdatePlayerPower();
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

    public void IdleRewardsTimeCheck()
    {
        TimeSpan timeLeft = DateTime.Now.Subtract(player.IdleCheckTime);

        if (timeLeft.TotalMinutes < 1 && Manager.Game.Player.ToTalIdleTime < 1)
        {
            _btnIdleRewards.interactable = false;
            StartCoroutine(DelayEnableButton(timeLeft));
        }
        else
        {
            _btnIdleRewards.interactable = true;
            _textIdleNotice.text = "";
        }
    }

    private IEnumerator DelayEnableButton(TimeSpan timeLeft)
    {
        TimeSpan remainTime = Delay.IdleClick - timeLeft;
        double totalSecondsLeft = remainTime.TotalSeconds;

        while (true)
        {
            if (totalSecondsLeft > 1)
            {
                if (totalSecondsLeft >= 60)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(totalSecondsLeft);
                    _textIdleNotice.text = $"{ts.Minutes}분 {ts.Seconds}초";
                }
                else
                {
                    _textIdleNotice.text = Mathf.FloorToInt((float)totalSecondsLeft) + "초";
                }

                totalSecondsLeft -= Time.deltaTime;
                yield return null;
            }
            else
            {
                _btnIdleRewards.interactable = true;
                _textIdleNotice.text = "";
                break;
            }
        }

        yield return null;
    }

    #endregion

    #region Button Events

    // 기능성 버튼 이벤트
    private void OnIdleRewards(PointerEventData eventData)
    {
        if (_btnIdleRewards.interactable) Manager.UI.ShowPopup<UIPopupRewardsIdle>("UIPopupRewardsIdle");
    }
    private void OnOption(PointerEventData eventData) => Manager.UI.ShowPopup<UIPopupOptions>("UIPopupOptions");
    private void OnBossStage(PointerEventData eventData) => Manager.Stage.RetryBossBattle();

    private void OnGameSpeed(PointerEventData eventData)
    {        
        if (isSpeedUp)
        {
            isSpeedUp = false;
            Time.timeScale = 1f;

            image_1xSpeed.gameObject.SetActive(true);
            image_2xSpeed.gameObject.SetActive(false);
        }
        else
        {
            isSpeedUp = true;
            Time.timeScale = 1.5f;

            image_1xSpeed.gameObject.SetActive(false);
            image_2xSpeed.gameObject.SetActive(true);
        }
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

    public void UpdateCurrentStage()
    {
        var stageUI = Manager.Stage.UITextReturn();
        txt_Stage.text = $"{stageUI.UIText} {Manager.Stage.Chapter - (stageUI.Index - 1)}층";
    }

    public void UpdateQuestNum()
    {
        _txtQuestNum.text = ($"퀘스트 {Manager.Quest.QuestNum + 1}");
    }

    public void UpdateQuestObjective()
    {
        _txtQuestObjective.text = QuestObjective();
    }

    public void UpdatePlayerPower()
    {
        var player = Manager.Game.Player;
        string v = Utilities.ConvertToString((long)(player.AtkDamage.Value
                        + player.AtkDamage.Value * player.EquipAttackStat / 100
                        + player.AtkDamage.Value * player.RetentionAttackEffect / 100));

        txt_PlayerPower.text = $"최종 공격력 : {v}";
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

    #region Quest

    public string QuestObjective()
    {
        if (Manager.Quest.CurrentQuest.questType == QuestType.StageClear)
        {
            return ($"{Manager.Quest.CurrentQuest.questObjective} {Manager.Quest.CurrentQuest.objectiveValue}");
        }
        else
        {
            return ($"{Manager.Quest.CurrentQuest.questObjective} {Manager.Quest.CurrentQuest.currentValue} / {Manager.Quest.CurrentQuest.objectiveValue}");
        }
    }

    #endregion
}
