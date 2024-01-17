using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private Player _player;

    private Button _checkBtn;
    private Button _summonTryBtn_11;
    private Button _summonTryBtn_35;
    private Button _closeBtn;

    #endregion

    #region Initialize

    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        SetButtonEvents();

        _player = Manager.Game.Player;
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        _checkBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
        _summonTryBtn_11 = SetButtonEvent("Btn_Summon_35Repeat", UIEventType.Click, SummonTry);
        _summonTryBtn_35 = SetButtonEvent("Btn_Summon_35Repeat", UIEventType.Click, SummonTry);
        _closeBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
    }

    #endregion


    #region Button Events

    private void CloseSummonPopup(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }

    private void SummonTry(PointerEventData eventData)
    {
        
    }

    #endregion
}
