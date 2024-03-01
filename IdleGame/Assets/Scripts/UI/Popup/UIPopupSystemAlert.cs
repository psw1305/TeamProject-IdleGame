using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupSystemAlert : UIPopup
{
    private PopupAlert _alertData;
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;
    private TextMeshProUGUI _confirmText;
    private Button _confirmButton;
    private Button _cancelButton;

    protected override void Init()
    {
        base.Init();
        
        SetButtonEvent();
    }


    private void SetButton()
    {
        SetUI<Button>();
        _confirmButton = GetUI<Button>("Btn_Confirm");
        _cancelButton = GetUI<Button>("Btn_Close");
    }

    private void SetButtonEvent()
    {
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    private void SetText()
    {
        SetUI<TextMeshProUGUI>();
        _titleText = GetUI<TextMeshProUGUI>("Text_Title");
        _descriptionText = GetUI<TextMeshProUGUI>("Text_Description");
        _confirmText = GetUI<TextMeshProUGUI>("Text_Confirm");
    }

    public void SetData(PopupAlertType alertType, Action action)
    {
        SetText();
        SetButton();
        _alertData = Manager.SysAlert.PopupAlerts[alertType];
        _titleText.text = _alertData.Title;
        _descriptionText.text = _alertData.Description;
        _confirmText.text = _alertData.ConfirmText;
        _confirmButton.onClick.AddListener(() => action?.Invoke());
        _confirmButton.gameObject.SetActive(true);
    }
    
    public void SetAlertData(PopupAlertType alertType)
    {
        SetText();
        SetButton();
        _alertData = Manager.SysAlert.PopupAlerts[alertType];
        _titleText.text = _alertData.Title;
        _descriptionText.text = _alertData.Description;
        _cancelButton.transform.localPosition = new Vector2(0, _cancelButton.transform.localPosition.y);
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
