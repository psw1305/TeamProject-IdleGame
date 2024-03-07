using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupRewardsIdle : UIPopup
{
    #region UI Fields

    private Button idleCheckBtn;
    private Button idleBonusBtn;

    private TextMeshProUGUI idleTimeText;
    private TextMeshProUGUI earnTimeText;
    private TextMeshProUGUI goldProvisionText;
    private TextMeshProUGUI bonusCheckText;

    #endregion

    #region Fields

    private Player player;
    private long idleGoldRewards;
    private DateTime bonusTimerStart;
    private DateTime bonusTimerEnd;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetTexts();
        SetButtonEvents();

        player = Manager.Game.Player;
        player.PopupUIInit(DisplayIdleRewards);

        DisplayIdleRewards();
        DisplayBonusCheck();
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        idleTimeText = GetUI<TextMeshProUGUI>("Text_Idle_Time");
        earnTimeText = GetUI<TextMeshProUGUI>("Text_Earn_Time");
        goldProvisionText = GetUI<TextMeshProUGUI>("Text_Gold_Provision");
        bonusCheckText = GetUI<TextMeshProUGUI>("Text_Bonus_Check");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("Btn_IdleReward_Check", UIEventType.Click, CheckIdleReward);
        SetButtonEvent("DimScreen", UIEventType.Click, CheckIdleReward);
        idleBonusBtn = SetButtonEvent("Btn_IdleReward_Bonus", UIEventType.Click, BonusIdleReward);
    }

    private void DisplayBonusCheck()
    {
        if (player.IsBonusCheck)
        {
            TimeSpan timeLeft = DateTime.Now.Subtract(player.BonusCheckTime);

            if (timeLeft.TotalMinutes >= 10)
            {
                player.SetBonusCheck(false);
                idleBonusBtn.interactable = true;
                bonusCheckText.text = "보너스";
            }
            else
            {
                StartCoroutine(DisplayBonusTimer());
            }
        }
    }

    #endregion

    #region Button Events

    private void CheckIdleReward(PointerEventData eventData)
    {
        player.RewardGold(player.ToTalIdleGold);
        player.SetIdleCheckTime(DateTime.Now);
        player.IdleRewardReset();

        UISceneMain mainUI = Manager.UI.CurrentScene as UISceneMain;
        mainUI.IdleRewardsTimeCheck();

        Manager.Data.Save();
        Manager.UI.ClosePopup();
    }

    private void BonusIdleReward(PointerEventData eventData)
    {
        if (!player.IsBonusCheck)
        {
            player.RewardGold(Manager.Stage.IdleGoldReward * 5);

            StartBonusTimer();
        } 
    }

    #endregion

    #region Idle Rewards Methods

    private void DisplayIdleRewards()
    {
        earnTimeText.text = Utilities.ConvertToString(Manager.Stage.IdleGoldReward) + "/m";
        idleTimeText.text = Manager.Game.Player.ToTalIdleTime.ToString();
        goldProvisionText.text = Utilities.ConvertToString(Manager.Game.Player.ToTalIdleGold);
    }

    #endregion

    #region Bonus Rewards Methods

    private void StartBonusTimer()
    {
        StartCoroutine(DisplayBonusTimer());
    }

    private IEnumerator DisplayBonusTimer()
    {
        idleBonusBtn.interactable = false;

        if (player.IsBonusCheck)
        {
            bonusTimerStart = player.BonusCheckTime;
            TimeSpan timeDelay = DateTime.Now.Subtract(bonusTimerStart);
            bonusTimerEnd = bonusTimerStart.Add(Delay.BonusClick - timeDelay);
        }
        else
        {
            player.SetBonusCheck(true);
            player.SetBonusCheckTime(DateTime.Now);
            bonusTimerStart = player.BonusCheckTime;
            bonusTimerEnd = bonusTimerStart.Add(Delay.BonusClick);
        }

        TimeSpan timeLeft = bonusTimerEnd.Subtract(bonusTimerStart);
        double totalSecondsLeft = timeLeft.TotalSeconds;
        
        while (true)
        {
            if (totalSecondsLeft > 1)
            {
                if (totalSecondsLeft >= 60)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(totalSecondsLeft);
                    bonusCheckText.text = $"{ts.Minutes}분 {ts.Seconds}초";
                }
                else
                {
                    bonusCheckText.text = Mathf.FloorToInt((float)totalSecondsLeft) + "초";
                }

                totalSecondsLeft -= Time.deltaTime;
                yield return null;
            }
            else
            {
                player.SetBonusCheck(false);
                idleBonusBtn.interactable = true;
                bonusCheckText.text = "보너스";
                break;
            }
        }

        yield return null;
    }

    #endregion
}
