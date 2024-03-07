using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupChangeNickname : UIPopup
{
    #region Fields

    private TMP_InputField inputField_Nickname;

    #endregion

    protected override void Init()
    {
        base.Init();

        SetInputField();
        SetButtonEvents();
    }

    private void SetInputField()
    {
        SetUI<TMP_InputField>();
        inputField_Nickname = GetUI<TMP_InputField>("InputField_Nickname");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("Btn_Check", UIEventType.Click, OnChangeNickname);
    }

    #region UI Events

    private void OnChangeNickname(PointerEventData eventData)
    {
        string newNickname = inputField_Nickname.text;
        Manager.Data.Profile.Nickname = newNickname;
        inputField_Nickname.text = "";
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    #endregion
}
