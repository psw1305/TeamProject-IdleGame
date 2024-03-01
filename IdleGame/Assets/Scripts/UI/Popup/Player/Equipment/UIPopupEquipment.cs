using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupEquipment : UIPopup
{
    #region Fleids

    private Image _itemImage;
    private Image _bgImage;
    private Image _typeIcon;
    private Image _reinforceProgress;

    private TextMeshProUGUI titleText;

    private TextMeshProUGUI _itemNameText;
    private TextMeshProUGUI _rarityText;
    private TextMeshProUGUI _itemLevelText;

    private TextMeshProUGUI _itemHasCount;

    private TextMeshProUGUI _equipEffect;
    private TextMeshProUGUI _retentionEffect;

    private Button btn_Select_Equip;
    private Button btn_Select_Reinforce;

    private UserItemData _selectItemData;
    private List<UserItemData> _fillterItems;
    private int _needCount;

    #endregion

    #region Properties

    private event Action RefreshReinforecEvent;

    public EquipFillterType _equipFillterType;

    public List<UserItemData> FillterItems => _fillterItems;

    #endregion

    #region UIBindMethod

    protected override void Init()
    {
        base.Init();

        SetText();
        SetImage();
        SetButtonEvents();

        SetItemTypeUI();
        FillterCurrentPopupUseItemData();
        SetFirstVisibleItem();
    }

    private void SetImage()
    {
        SetUI<Image>();
        _itemImage = GetUI<Image>("Img_EquipSlot");
        _bgImage = GetUI<Image>("Img_EquipSlotBG");
        _typeIcon = GetUI<Image>("Img_ETypeIcon");
        _reinforceProgress = GetUI<Image>("Img_ReinforceProgress");
    }

    private void SetText()
    {
        SetUI<TextMeshProUGUI>();
        titleText = GetUI<TextMeshProUGUI>("Text_Title");
        _itemNameText = GetUI<TextMeshProUGUI>("Text_EquipName");
        _itemLevelText = GetUI<TextMeshProUGUI>("Text_Lv");
        _rarityText = GetUI<TextMeshProUGUI>("Text_Rarity");
        _itemHasCount = GetUI<TextMeshProUGUI>("Text_hasCount");
        _equipEffect = GetUI<TextMeshProUGUI>("Text_EquipStat");
        _retentionEffect = GetUI<TextMeshProUGUI>("Text_RetentionStat");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();

        btn_Select_Equip = SetButtonEvent("Btn_Equip", UIEventType.Click, EquipmentSelectItem);
        btn_Select_Reinforce = SetButtonEvent("Btn_Reinforce", UIEventType.Click, ReinforceSelectItem);

        SetButtonEvent("Btn_ReinforceType", UIEventType.Click, ReinforceWeaponTypeItem);
        SetButtonEvent("Btn_ShowSummon", UIEventType.Click, ShowSummonScene);
        SetButtonEvent("Btn_RecommendEquip", UIEventType.Click, EquipmentRecommendItem);
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    #endregion

    #region OtherMethod

    //EquipFillterType 상태에 맞춰 보여주는 장비류 필터를 Inventory Manager로부터 가져옵니다.
    private void FillterCurrentPopupUseItemData()
    {
        if (_equipFillterType == EquipFillterType.Weapon)
        {
            _fillterItems = Manager.Inventory.WeaponItemList;
        }
        else if (_equipFillterType == EquipFillterType.Armor)
        {
            _fillterItems = Manager.Inventory.ArmorItemList;
        }
    }

    //팝업 첫 진입 시 조건에 따라 처음 아이템을 설정해줍니다.
    private void SetFirstVisibleItem()
    {
        //아무것도 착용하지 않은 경우 제일 첫 아이템
        if (_fillterItems.Where(item => item.equipped == true).ToList().Count == 0)
        {
            SetSelectItemInfo(_fillterItems[0]);
        }
        //착용한 경우 착용한 아이템
        else
        {
            SetSelectItemInfo(_fillterItems.Where(item => item.equipped == true).ToList()[0]);
        }
    }

    //아이템 타입에 따라 UI가 세팅되는 메서드
    private void SetItemTypeUI()
    {
        if (_equipFillterType == EquipFillterType.Weapon)
        {
            titleText.text = "Weapon";
        }
        else if (_equipFillterType == EquipFillterType.Armor)
        {
            titleText.text = "Armor";
        }
    }

    //강화에 필요한아이템 개수를 계산합니다.
    private void CalculateNeedItemCount()
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

    // 선택한 아이템의 정보를 상단 UI에 설정하는 메서드입니다.
    public void SetSelectItemInfo(UserItemData selectItemData)
    {
        //UI 정보를 세팅합니다.
        _selectItemData = selectItemData;
        _itemNameText.text = Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ItemName;

        _rarityText.color = Utilities.SetSlotTierColor(Manager.Inventory.ItemDataDictionary[selectItemData.itemID].Rarity);
        _rarityText.text = Manager.Inventory.ItemDataDictionary[selectItemData.itemID].Rarity.ToString();

        _itemLevelText.text = _selectItemData.level.ToString();

        _bgImage.color = Utilities.SetSlotTierColor(Manager.Inventory.ItemDataDictionary[selectItemData.itemID].Rarity);
        _itemImage.sprite = Manager.Inventory.ItemDataDictionary[_selectItemData.itemID].Sprite;

        CalculateNeedItemCount();
        _itemHasCount.text = $"{_selectItemData.hasCount} / {_needCount}";
        _reinforceProgress.fillAmount = (float)_selectItemData.hasCount / _needCount;


        if (Manager.Inventory.ItemDataDictionary[selectItemData.itemID].StatType == StatType.Attack)
        {
            _equipEffect.text = $"공격력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].EquipStat + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"공격력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].RetentionEffect + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEffect * _selectItemData.level}%";
        }
        else if (Manager.Inventory.ItemDataDictionary[selectItemData.itemID].StatType == StatType.HP)
        {
            _equipEffect.text = $"체력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].EquipStat + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"체력 :  {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].RetentionEffect + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEffect * _selectItemData.level}%";
        }
        SetEquipBtn(_selectItemData);
        SetReinforceBtn(_selectItemData);
    }

    private void SetEquipBtn(UserItemData userItemData)
    {
        btn_Select_Equip.interactable = userItemData.level > 1 | userItemData.hasCount > 0;
    }

    private void SetReinforceBtn(UserItemData userItemData)
    {
        if (userItemData.itemID == _fillterItems.Last().itemID & userItemData.level >= 100)
        {
            btn_Select_Reinforce.interactable = false;
        }
        else
        {
            btn_Select_Reinforce.interactable = userItemData.hasCount >= MathF.Min(userItemData.level + 1, 15);
        }
    }

    //선택한 아이템을 착용합니다.
    private void EquipmentSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ChangeEquipmentItem(_selectItemData);
        Manager.Notificate.SetPlayerStateNoti();

        Manager.Notificate.SetRecommendWeaponNoti();
        Manager.Notificate.SetRecommendArmorNoti();

        Manager.Notificate.SetWeaponEquipmentNoti();
        Manager.Notificate.SetArmorEquipmentNoti();

        CallEquipRefreshEvent();
        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    private void EquipmentRecommendItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ChangeEquipmentItem(Manager.Notificate.CheckRecommendItem(_fillterItems));

        if (_equipFillterType == EquipFillterType.Weapon)
        {
            Manager.Notificate.SetRecommendWeaponNoti();
            Manager.Notificate.SetWeaponEquipmentNoti();
        }
        else if (_equipFillterType == EquipFillterType.Armor)
        {
            Manager.Notificate.SetRecommendArmorNoti();
            Manager.Notificate.SetArmorEquipmentNoti();
        }

        CallEquipRefreshEvent();
        Manager.Notificate.SetPlayerStateNoti();
        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }


    public void SubscribeEquipRefreshEvent(Action action)
    {
        RefreshReinforecEvent += action;
    }
    public void UnsubscribeEquipRefreshEvent(Action action)
    {
        RefreshReinforecEvent -= action;
    }

    private void CallEquipRefreshEvent()
    {
        RefreshReinforecEvent?.Invoke();
    }

    //선택한 아이템을 강화합니다.
    private void ReinforceSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectItem(_selectItemData);
        if (_selectItemData.itemID[0] == 'W')
        {
            Manager.Notificate.SetRecommendWeaponNoti();
            Manager.Notificate.SetWeaponEquipmentNoti();
            Manager.Notificate.SetReinforceWeaponNoti();
        }
        else
        {
            Manager.Notificate.SetRecommendArmorNoti();
            Manager.Notificate.SetReinforceArmorNoti();
            Manager.Notificate.SetArmorEquipmentNoti();
        }

        Manager.Notificate.SetPlayerStateNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    //무기 종류 일괄 강화
    private void ReinforceWeaponTypeItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(_fillterItems);

        if (_equipFillterType == EquipFillterType.Weapon)
        {
            Manager.Notificate.SetRecommendWeaponNoti();
            Manager.Notificate.SetWeaponEquipmentNoti();
            Manager.Notificate.SetReinforceWeaponNoti();
        }
        else
        {
            Manager.Notificate.SetRecommendArmorNoti();
            Manager.Notificate.SetArmorEquipmentNoti();
            Manager.Notificate.SetReinforceArmorNoti();
        }
        Manager.Notificate.SetPlayerStateNoti();

        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    private void CallReinforceRefreshEvent()
    {
        RefreshReinforecEvent?.Invoke();

        // Data 저장
        Manager.Data.Save();
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

    #endregion

    #region Unity Flow

    private void OnDestroy()
    {
        RefreshReinforecEvent = null;
    }

    #endregion
}