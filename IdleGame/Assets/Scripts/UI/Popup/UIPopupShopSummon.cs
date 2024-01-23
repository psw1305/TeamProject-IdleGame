using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private SummonManager _summon;

    private Button _checkBtn;
    private Button _summonTryBtn_Ad;
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

        _summon = Manager.Summon;
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        _summonTryBtn_Ad = SetButtonEvent("Btn_Summon_AdRepeat", UIEventType.Click, SummonRepeat_Ad);
        _summonTryBtn_11 = SetButtonEvent("Btn_Summon_11Repeat", UIEventType.Click, SummonRepeat_1);
        _summonTryBtn_35 = SetButtonEvent("Btn_Summon_35Repeat", UIEventType.Click, SummonRepeat_2);
        _closeBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
    }

    #endregion

    #region Button Events

    private void SummonRepeat_Ad(PointerEventData eventData)
    {
        _summon.SummonTry(0, 11);
    }

    private void SummonRepeat_1(PointerEventData eventData)
    {
        _summon.SummonTry(500, 11);
    }

    private void SummonRepeat_2(PointerEventData eventData)
    {
        _summon.SummonTry(1500, 35);
    }


    private void CloseSummonPopup(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }
    #endregion
}
