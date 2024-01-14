using UnityEngine.EventSystems;

public class UIPopupEquipment : UIPopup
{
    // 팝업 닫기
    private void ExitPopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
