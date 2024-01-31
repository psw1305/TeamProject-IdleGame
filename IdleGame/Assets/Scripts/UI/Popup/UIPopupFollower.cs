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

    public event Action RefreshReinforecEvent;

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

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    // TODO : follower data 가져오기
    private void OperNeedItemCount()
    {
        if (_selectItemData.level < 15)
        {
            _needCount = _selectItemData.level + 1;
        }
        else
        {
            _needCount = 15;
        }
    }

    public void SetSelectItemInfo(UserItemData selectItemData)
    {

    }

    private void CallReinforceRefreshEvent()
    {
        RefreshReinforecEvent?.Invoke();
    }

    private void ReinforceFollowers(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(Manager.Inventory.WeaponItemList);
        // TODO: 강화 알리미 추가 
        //Manager.NotificateDot.SetRecommendWeaponNoti();
        //Manager.NotificateDot.SetWeaponEquipmentNoti();
        //Manager.NotificateDot.SetReinforceWeaponNoti();
        //Manager.NotificateDot.SetEquipmentNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}