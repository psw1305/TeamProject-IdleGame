using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIPopupOptionDropdownPanel : UIPopup
{
    #region Fields

    private Button _optionBtn_Notice;
    private Button _optionBtn_Setting;
    private Button _optionBtn_MailBox;
    private Button _optionBtn_Inventory;
    private Button _optionBtn_GameInfo;
    private Button _optionBtn_Exit;
    private Button _optionBtn_OutOfArea;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        SetButtons();
        SetEvents();
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _optionBtn_Notice = GetUI<Button>("Option_Notice_Btn");
        _optionBtn_Setting = GetUI<Button>("Option_Setting_Btn");
        _optionBtn_MailBox = GetUI<Button>("Option_MailBox_Btn");
        _optionBtn_Inventory = GetUI<Button>("Option_Inventory_Btn");
        _optionBtn_GameInfo = GetUI<Button>("Option_GameInfo_Btn");
        _optionBtn_Exit = GetUI<Button>("Option_CloseBtn");
        _optionBtn_OutOfArea = GetUI<Button>("DimScreen");
    }

    private void SetEvents()
    {
        _optionBtn_Notice.gameObject.SetEvent(UIEventType.Click, OnNoticePopup);
        _optionBtn_Setting.gameObject.SetEvent(UIEventType.Click, OnSettingPopup);
        _optionBtn_MailBox.gameObject.SetEvent(UIEventType.Click, OnMailBoxPopup);
        _optionBtn_Inventory.gameObject.SetEvent(UIEventType.Click, OnInventoryPopup);
        _optionBtn_GameInfo.gameObject.SetEvent(UIEventType.Click, OnGameInfoPopup);

        // 나가기
        _optionBtn_Exit.gameObject.SetEvent(UIEventType.Click, ClosePopup);
        _optionBtn_OutOfArea.gameObject.SetEvent(UIEventType.Click, ClosePopup);
    }

    #endregion


    #region Button Events

    private void OnNoticePopup(PointerEventData eventData)
    {
        // 추후 공지 연결
        // 임시로 방치 보상 연결
        Manager.UI.ShowPopup<UIPopupIdleRewardPopup>("IdleRewardPopup");
    }
    private void OnGameInfoPopup(PointerEventData eventData)
    {
        // 추후 게임 정보 연결
        Debug.Log("GameInfo Open");
    }

    private void OnInventoryPopup(PointerEventData eventData)
    {
        // 추후 인벤토리 연결
        Debug.Log("Inventory Open");
    }

    private void OnMailBoxPopup(PointerEventData eventData)
    {
        // 추후 우편함 연결
        Debug.Log("MailBox Open");
    }

    private void OnSettingPopup(PointerEventData eventData)
    {
        // 추후 설정 연결
        Debug.Log("Setting Open");
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    #endregion
}
