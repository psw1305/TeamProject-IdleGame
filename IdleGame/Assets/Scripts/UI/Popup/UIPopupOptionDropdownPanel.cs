using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupOptionDropdownPanel : UIPopup
{
    #region Fields

    private Button _optionBtn_Notice;
    private Button _optionBtn_Setting;
    private Button _optionBtn_MailBox;
    private Button _optionBtn_Inventory;
    private Button _optionBtn_GameQuit;
    private Button _optionBtn_Exit;
    private Button _optionBtn_OutOfArea;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        _optionBtn_Notice = SetButtonEvent("Option_Notice_Btn", UIEventType.Click, OnNoticePopup);
        _optionBtn_Setting = SetButtonEvent("Option_Setting_Btn", UIEventType.Click, OnSettingPopup);
        _optionBtn_MailBox = SetButtonEvent("Option_MailBox_Btn", UIEventType.Click, OnMailBoxPopup);
        _optionBtn_Inventory = SetButtonEvent("Option_Inventory_Btn", UIEventType.Click, OnInventoryPopup);
        _optionBtn_GameQuit = SetButtonEvent("Option_Quit_Btn", UIEventType.Click, GameQuit);
        _optionBtn_Exit = SetButtonEvent("Option_CloseBtn", UIEventType.Click, ClosePopup);
        _optionBtn_OutOfArea = SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    #endregion

    #region Button Events

    private void OnNoticePopup(PointerEventData eventData)
    {
        // 추후 공지 연결
    }

    private void OnSettingPopup(PointerEventData eventData)
    {
        // 추후 설정 연결
    }
    private void OnMailBoxPopup(PointerEventData eventData)
    {
        // 추후 우편함 연결
        Debug.Log("MailBox Open");
    }
    private void OnInventoryPopup(PointerEventData eventData)
    {
        // 추후 인벤토리 연결
        Debug.Log("Inventory Open");
    }

    private void GameQuit(PointerEventData eventData)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

#endregion
}
