using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupOptions : UIPopup
{
    #region Initialize

    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("Option_Notice_Btn", UIEventType.Click, OnNoticePopup);
        SetButtonEvent("Option_Setting_Btn", UIEventType.Click, OnSettingPopup);
        SetButtonEvent("Option_MailBox_Btn", UIEventType.Click, OnMailBoxPopup);
        SetButtonEvent("Option_Inventory_Btn", UIEventType.Click, OnInventoryPopup);
        SetButtonEvent("Option_Quit_Btn", UIEventType.Click, ShowQuitPopup);

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    #endregion

    #region Button Events

    private void OnNoticePopup(PointerEventData eventData)
    {
        // 추후 공지 연결
        var alertPopup = Manager.UI.ShowPopup<UIPopupSystemAlert>();
        alertPopup.SetAlertData(PopupAlertType.DevelopingContent);
    }

    private void OnSettingPopup(PointerEventData eventData) => Manager.UI.ShowPopup<UIPopupSettings>();

    private void OnMailBoxPopup(PointerEventData eventData)
    {
        // 추후 우편함 연결
        var alertPopup = Manager.UI.ShowPopup<UIPopupSystemAlert>();
        alertPopup.SetAlertData(PopupAlertType.DevelopingContent);
    }
    private void OnInventoryPopup(PointerEventData eventData)
    {
        // 추후 인벤토리 연결
        var alertPopup = Manager.UI.ShowPopup<UIPopupSystemAlert>();
        alertPopup.SetAlertData(PopupAlertType.DevelopingContent);
    }

    private void ShowQuitPopup(PointerEventData eventData)
    {
        var alertPopup = Manager.UI.ShowPopup<UIPopupSystemAlert>();
        alertPopup.SetData(PopupAlertType.ApplicationQuit, GameQuit);
    }

    private void GameQuit()
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
