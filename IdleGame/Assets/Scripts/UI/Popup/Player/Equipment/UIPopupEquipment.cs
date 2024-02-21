using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupEquipment : UIPopup
{
    #region Fleids

    private Image _itemImage;
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
    private Button btn_Reinforce_Weapon;
    private Button btn_Reinforce_Armor;

    private UserItemData _selectItemData;
    private List<UserItemData> _fillterItems;
    private int _needCount;

    #endregion

    #region Properties

    public event Action RefreshReinforecEvent;

    public EquipFillterType EquipFillterType;

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

        btn_Reinforce_Weapon = SetButtonEvent("Btn_ReinforceWeaponType", UIEventType.Click, ReinforceWeaponTypeItem);
        btn_Reinforce_Armor = SetButtonEvent("Btn_ReinforceArmorType", UIEventType.Click, ReinforceArmorTypeItem);

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    #endregion

    #region OtherMethod

    //강화에 필요한아이템 개수를 계산합니다.
    private void CalculateNeedItemCount()
    {
        if(_selectItemData.level < 15)
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
        _rarityText.text = Utilities.ConvertTierString(Manager.Inventory.ItemDataDictionary[selectItemData.itemID].Rarity);
        _itemLevelText.text = _selectItemData.level.ToString();

        _itemImage.sprite = Manager.Inventory.ItemDataDictionary[_selectItemData.itemID].Sprite;

        CalculateNeedItemCount();
        _itemHasCount.text = $"{_selectItemData.hasCount} / {_needCount}";
        _reinforceProgress.fillAmount = (float)_selectItemData.hasCount / _needCount;


        if (Manager.Inventory.ItemDataDictionary[selectItemData.itemID].StatType == "attack")
        {
            _equipEffect.text = $"공격력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].EquipStat + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"공격력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].RetentionEffect + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEffect * _selectItemData.level}%";
        }
        else if(Manager.Inventory.ItemDataDictionary[selectItemData.itemID].StatType == "hp")
        {
            _equipEffect.text = $"체력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].EquipStat + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"체력 :  {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].RetentionEffect + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].ReinforceEffect * _selectItemData.level}%";
        }

        if (_selectItemData.level == 1 && _selectItemData.hasCount == 0)
        {
            btn_Select_Equip.interactable = false;
            btn_Select_Reinforce.interactable= false;
        }
        else
        {
            btn_Select_Equip.interactable = true;
            btn_Select_Reinforce.interactable = (_selectItemData.hasCount >= _needCount);
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
    }
    private void CallEquipRefreshEvent()
    {
        RefreshReinforecEvent?.Invoke();
    }

    //선택한 아이템을 강화합니다.
    private void ReinforceSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectItem(_selectItemData);

        Manager.Notificate.SetReinforceWeaponNoti();
        Manager.Notificate.SetReinforceArmorNoti();

        Manager.Notificate.SetRecommendWeaponNoti();
        Manager.Notificate.SetRecommendArmorNoti();

        Manager.Notificate.SetWeaponEquipmentNoti();
        Manager.Notificate.SetArmorEquipmentNoti();

        Manager.Notificate.SetPlayerStateNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    //무기 종류 일괄 강화
    private void ReinforceWeaponTypeItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(Manager.Inventory.WeaponItemList);
        //이후 각각 조건에 맞춰 버튼에 알람이 활성화 되어야 하는지, 종료되어야 하는지에 대한 정보를 뿌립니다.
        Manager.Notificate.SetRecommendWeaponNoti();
        Manager.Notificate.SetWeaponEquipmentNoti();
        Manager.Notificate.SetReinforceWeaponNoti();
        Manager.Notificate.SetPlayerStateNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    //방어구 종류 일괄 강화
    private void ReinforceArmorTypeItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(Manager.Inventory.ArmorItemList);
        //이후 각각 조건에 맞춰 버튼에 알람이 활성화 되어야 하는지, 종료되어야 하는지에 대한 정보를 뿌립니다.
        Manager.Notificate.SetRecommendArmorNoti();
        Manager.Notificate.SetArmorEquipmentNoti();
        Manager.Notificate.SetReinforceArmorNoti();
        Manager.Notificate.SetPlayerStateNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    private void CallReinforceRefreshEvent()
    {
        RefreshReinforecEvent?.Invoke();

        // Data 저장
        Manager.Data.Save();
    }

    //EquipFillterType 상태에 맞춰 보여주는 장비류 필터를 Inventory Manager로부터 가져옵니다.
    private void FillterCurrentPopupUseItemData()
    {
        if (EquipFillterType == EquipFillterType.Weapon)
        {
            _fillterItems = Manager.Inventory.WeaponItemList;
        }
        else if(EquipFillterType == EquipFillterType.Armor)
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
        if (EquipFillterType == EquipFillterType.Weapon)
        {
            titleText.text = "Weapon";
            btn_Reinforce_Weapon.gameObject.SetActive(true);
            btn_Reinforce_Armor.gameObject.SetActive(false);
        }
        else if (EquipFillterType == EquipFillterType.Armor)
        {
            titleText.text = "Skill";
            btn_Reinforce_Weapon.gameObject.SetActive(false);
            btn_Reinforce_Armor.gameObject.SetActive(true);
        }
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