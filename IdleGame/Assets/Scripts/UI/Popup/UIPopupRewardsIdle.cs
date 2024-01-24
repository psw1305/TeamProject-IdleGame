using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupRewardsIdle : UIPopup
{
    #region UI Fields

    private Button checkBtn;
    private Button bonusBtn;

    private TextMeshProUGUI idleTimeText;
    private TextMeshProUGUI bonusCheckText;

    #endregion

    #region Fields

    private Player player;
    private DateTime timerStart;
    private DateTime timerEnd;
    private readonly TimeSpan bonusDelayTime = new(0, 10, 0);

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetTexts();
        SetButtonEvents();

        BonusTimeCheck();
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        idleTimeText = GetUI<TextMeshProUGUI>("Text_Idle_Time");
        bonusCheckText = GetUI<TextMeshProUGUI>("Text_Bonus_Check");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        checkBtn = SetButtonEvent("Btn_IdleReward_Check", UIEventType.Click, CheckIdleReward);
        bonusBtn = SetButtonEvent("Btn_IdleReward_Bonus", UIEventType.Click, BonusIdleReward);
    }

    private void BonusTimeCheck()
    {
        player = Manager.Game.Player;

        if (player.IsBonusCheck)
        {
            TimeSpan timeLeft = DateTime.Now.Subtract(player.BonusCheckTime);

            if (timeLeft.TotalMinutes >= 10)
            {
                player.SetBonusCheck(false);
                bonusBtn.interactable = true;
                bonusCheckText.text = "보너스";
            }
            else
            {
                StartCoroutine(DisplayTime());
            }
        }
    }

    #endregion

    #region Button Events

    private void CheckIdleReward(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }

    private void BonusIdleReward(PointerEventData eventData)
    {
        if (!player.IsBonusCheck) StartTimer();
    }

    #endregion

    #region Time Events

    private void StartTimer()
    {
        StartCoroutine(DisplayTime());
    }

    private IEnumerator DisplayTime()
    {
        bonusBtn.interactable = false;

        if (player.IsBonusCheck)
        {
            timerStart = player.BonusCheckTime;
            TimeSpan timeDelay = DateTime.Now.Subtract(timerStart);
            timerEnd = timerStart.Add(bonusDelayTime - timeDelay);
        }
        else
        {
            player.SetBonusCheck(true);
            player.SetBonusTime(DateTime.Now);
            timerStart = player.BonusCheckTime;
            timerEnd = timerStart.Add(bonusDelayTime);
        }

        TimeSpan timeLeft = timerEnd.Subtract(timerStart);
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
                bonusBtn.interactable = true;
                bonusCheckText.text = "보너스";
                break;
            }
        }

        yield return null;
    }

    #endregion
}
