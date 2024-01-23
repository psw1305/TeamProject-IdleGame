using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupIdleReward : UIPopup
{
    #region Fields

    private Button checkBtn;

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
}
