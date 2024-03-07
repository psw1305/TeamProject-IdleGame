using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollower : UIPopup
{
    #region Fleids

    private Image[] _followerImages = new Image[5];

    private Button Btn_ReinforceFollower;

    // TODO :: FollowerData
    private UserItemData _selectItemData; 
    private int _needCount;

    #endregion

    protected override void Init()
    {
        base.Init();

        SetImage();
        SetButtonEvents();

    }

    private void SetImage()
    {
        SetUI<Image>();
        _followerImages[0] = GetUI<Image>("Img_EquipSlot 1");
        _followerImages[1] = GetUI<Image>("Img_EquipSlot 2");
        _followerImages[2] = GetUI<Image>("Img_EquipSlot 3");
        _followerImages[3] = GetUI<Image>("Img_EquipSlot 4");
        _followerImages[4] = GetUI<Image>("Img_EquipSlot 5");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();

        Btn_ReinforceFollower = SetButtonEvent("Btn_ReinforceFollower", UIEventType.Click, ReinforceFollowers);

        SetButtonEvent("Btn_ShowSummon", UIEventType.Click, ShowSummonScene);
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    private void ReinforceFollowers(PointerEventData enterEvent)
    {
        Manager.FollowerData.ReinforceAllFollower();
    }

    private void ShowSummonScene(PointerEventData eventData)
    {
        Manager.UI.ShowSubScene<UISubSceneShopSummon>();
        Manager.UI.ClosePopup();
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}