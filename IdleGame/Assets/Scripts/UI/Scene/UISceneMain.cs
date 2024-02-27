using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISceneMain : UIScene
{
    #region Serialize Fields

    [SerializeField] private UIUpgradeStat[] upgradeStats;

    #endregion

    #region Player Fields

    private Player player;

    private TextMeshProUGUI txt_Stage;
    private TextMeshProUGUI txt_PlayerPower;

    private Image image_WaveLoop;
    private Image image_LevelGauge;
    private Image image_1xSpeed;
    private Image image_2xSpeed;

    #endregion

    #region Fields

    private Button btnBoss;
    private Button btnIdleRewards;
    private Toggle toggleGameSpeed;

    private TextMeshProUGUI _txtQuestNum;
    private TextMeshProUGUI _txtQuestObjective;
    private TextMeshProUGUI _textIdleNotice;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetImages();
        SetTexts();
        SetButtons();
        SetToggles();

        player = Manager.Game.Player;

        SetUpgradeStats();
        SetUI();

        IdleRewardsTimeCheck();
    }

    private void SetImages()
    {
        SetUI<Image>();
        image_WaveLoop = GetUI<Image>("LoopImage");
        image_LevelGauge = GetUI<Image>("ProgressGauge");
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
        SetButtonEvent("Btn_Options", UIEventType.Click, OnOption);
        SetButtonEvent("Btn_Quest", UIEventType.Click, OnQuestComplete);

        btnBoss = SetButtonEvent("Btn_Boss", UIEventType.Click, OnBossStage);
        btnIdleRewards = SetButtonEvent("Btn_IdleRewards", UIEventType.Click, OnIdleRewards);
    }

    private void SetToggles()
    {
        SetUI<Toggle>();
        toggleGameSpeed = GetUI<Toggle>("Toggle_GameSpeed");
    }

    private void SetUpgradeStats()
    {
        upgradeStats[0].SetUpgradeStat(player.Hp, this);
        upgradeStats[1].SetUpgradeStat(player.HpRecovery, this);
        upgradeStats[2].SetUpgradeStat(player.AtkDamage, this);
        upgradeStats[3].SetUpgradeStat(player.AtkSpeed, this);
        upgradeStats[4].SetUpgradeStat(player.CritChance, this);
        upgradeStats[5].SetUpgradeStat(player.CritDamage, this);

        UpdateStatTradeCheck();
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
        btnBoss.gameObject.SetActive(false);
        image_WaveLoop.gameObject.SetActive(false);

        if (Manager.Stage.WaveLoop)
        {
            RetryBossButtonToggle();
            WaveLoopImageToggle();
            StageLevelGaugeToggle(false);
        }

        UpdateStageLevel(Manager.Stage.StageLevel);

        // Game Speed
        SetGameSpeed();
    }

    private void SetGameSpeed()
    {
        toggleGameSpeed.onValueChanged.AddListener(OnChangedGameSpeed);

        if (PlayerPrefs.GetInt("GameSpeed", 0) == 1)
        {
            toggleGameSpeed.isOn = true;
        }
        else
        {
            toggleGameSpeed.isOn = false;
        }
    }

    public void IdleRewardsTimeCheck()
    {
        TimeSpan timeLeft = DateTime.Now.Subtract(player.IdleCheckTime);

        if (timeLeft.TotalMinutes < 1 && Manager.Game.Player.ToTalIdleTime < 1)
        {
            btnIdleRewards.interactable = false;
            StartCoroutine(DelayEnableButton(timeLeft));
        }
        else
        {
            btnIdleRewards.interactable = true;
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
                btnIdleRewards.interactable = true;
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
        if (btnIdleRewards.interactable) Manager.UI.ShowPopup<UIPopupRewardsIdle>("UIPopupRewardsIdle");
    }
    private void OnOption(PointerEventData eventData) => Manager.UI.ShowPopup<UIPopupOptions>("UIPopupOptions");
    private void OnBossStage(PointerEventData eventData) => Manager.Stage.RetryBossBattle();

    private void OnChangedGameSpeed(bool isOn)
    {
        if (isOn)
        {
            Time.timeScale = 1.5f;
            
            image_1xSpeed.gameObject.SetActive(false);
            image_2xSpeed.gameObject.SetActive(true);
            PlayerPrefs.SetInt("GameSpeed", 1);
        }
        else
        {
            Time.timeScale = 1f;

            image_1xSpeed.gameObject.SetActive(true);
            image_2xSpeed.gameObject.SetActive(false);
            PlayerPrefs.SetInt("GameSpeed", 0);
        }
    }

    private void OnQuestComplete(PointerEventData eventData)
    {
        if (Manager.Quest.IsQuestComplete())
        {
            Manager.Quest.EarnQuestReward();
            Manager.Quest.NextQuest();
            UpdateQuestNum();
            UpdateQuestObjective();
        }
    }

    #endregion

    #region Update UI

    public void UpdateStatTradeCheck()
    {
        foreach (var upgradeStat in upgradeStats)
        {
            upgradeStat.TradeCheck();
        }
    }

    public void UpdateStatLayoutChange(GameObject child)
    {
        child.transform.SetSiblingIndex(upgradeStats.Length - 1);
    }

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
        image_LevelGauge.fillAmount = level / 4.0f;
    }

    public void RetryBossButtonToggle()
    {
        btnBoss.gameObject.SetActive(!btnBoss.IsActive());
    }

    public void WaveLoopImageToggle()
    {
        image_WaveLoop.gameObject.SetActive(!image_WaveLoop.IsActive());
    }

    public void StageLevelGaugeToggle()
    {
        var gauge = image_LevelGauge.transform.parent.gameObject;
        gauge.SetActive(!gauge.activeSelf);
    }

    public void StageLevelGaugeToggle(bool active)
    {
        var gauge = image_LevelGauge.transform.parent.gameObject;
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
