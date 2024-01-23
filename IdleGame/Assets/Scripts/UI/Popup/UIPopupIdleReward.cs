using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupIdleReward : UIPopup
{
    #region Fields

    [SerializeField] private int days;
    [SerializeField] private int hours;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;

    [SerializeField] private TextMeshProUGUI timeText;

    private Button checkBtn;
    private DateTime timerStart;
    private DateTime timerEnd;

    #endregion

    #region Initialize

    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        SetButtonEvents();

        StartTimer();
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        checkBtn = SetButtonEvent("Btn_IdleReward_Check", UIEventType.Click, CheckIdleReward);
    }

    #endregion

    #region Button Events

    private void CheckIdleReward(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }

    #endregion

    #region Time Events

    private void StartTimer()
    {
        timerStart = DateTime.Now;
        TimeSpan time = new TimeSpan(days, hours, minutes, seconds);
        timerEnd = timerStart.Add(time);
        timeText.text = $"{timerStart} => {timerEnd}";
    }

    #endregion
}
