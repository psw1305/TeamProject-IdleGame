using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupIdleRewardPopup : UIPopup
{
    #region Fields

    private Button _checkBtn;

    #endregion

    #region Initialize

    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        _checkBtn = SetButtonEvent("Common_Button_Check", UIEventType.Click, IdleRewardCheck);
    }

    #endregion


    #region Button Events

    private void IdleRewardCheck(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }

    #endregion
}
