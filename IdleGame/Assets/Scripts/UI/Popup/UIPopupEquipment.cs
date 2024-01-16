using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class UIPopupEquipment : UIPopup
{
    private Button _btnExit;


    protected override void Init()
    {
        base.Init();
        SetButtons();
        SetEvents();
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _btnExit = GetUI<Button>("Btn_PopClose");
    }
    private void SetEvents()
    {
        _btnExit.gameObject.SetEvent(UIEventType.Click, ExitPopup);
    }

    // 팝업 닫기
    private void ExitPopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
