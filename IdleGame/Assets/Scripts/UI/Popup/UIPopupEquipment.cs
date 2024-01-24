using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using static UnityEditor.Progress;

public class UIPopupEquipment : UIPopup
{
    #region fleids
    private Image _itemImage;
    private Image _typeIcon;

    private TextMeshProUGUI _PopupTitle;

    private TextMeshProUGUI _itemNameText;
    private TextMeshProUGUI _rarityText;
    private TextMeshProUGUI _itemLevelText;

    private Image _reinforceProgress;
    private TextMeshProUGUI _itemHasCount;

    private TextMeshProUGUI _equipEffect;
    private TextMeshProUGUI _retentionEffect;

    private Button _BtnSelectEquip;
    private Button _btnSelectReinforce;

    private Button _btnExit;
    private Button _btnWeaponTypeReinforce;
    private Button _btnArmorTypeReinforce;

    private Button _btnTestWeapon;
    private Button _btnTestArmor;

    private Image _itemContainer;

    private InventorySlotData _selectItemData;

    private List<InventorySlotData> _fillterItems;

    private int _needCount;
    #endregion

    #region Properties

    public event Action refreshReinforecEvent;

    public EquipFillterType equipFillterType;

    public List<InventorySlotData> FillterItems => _fillterItems;

    #endregion

    #region UIBindMethod

    protected override void Init()
    {
        base.Init();

        SetText();
        SetImage();
        SetButtons();
        SetEvents();
        equipFillterType = EquipFillterType.Weapon;
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
        _itemContainer = GetUI<Image>("ItemContainer");
    }

    private void SetText()
    {
        SetUI<TextMeshProUGUI>();
        _PopupTitle = GetUI<TextMeshProUGUI>("TitleTxt");
        _itemNameText = GetUI<TextMeshProUGUI>("Text_EquipName");
        _itemLevelText = GetUI<TextMeshProUGUI>("Text_Lv");
        _rarityText = GetUI<TextMeshProUGUI>("Text_Rarity");
        _itemHasCount = GetUI<TextMeshProUGUI>("Text_hasCount");
        _equipEffect = GetUI<TextMeshProUGUI>("Text_EquipStat");
        _retentionEffect = GetUI<TextMeshProUGUI>("Text_RetentionStat");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        _btnExit = GetUI<Button>("Btn_PopClose");
        _BtnSelectEquip = GetUI<Button>("Btn_Equip");
        _btnSelectReinforce = GetUI<Button>("Btn_Reinforce");
        _btnWeaponTypeReinforce = GetUI<Button>("Btn_ReinforceWeaponType");
        _btnArmorTypeReinforce = GetUI<Button>("Btn_ReinforceArmorType");
        _btnTestWeapon = GetUI<Button>("Btn_TestWeapon");
        _btnTestArmor = GetUI<Button>("Btn_TestArmor");
    }

    private void SetEvents()
    {
        _btnExit.gameObject.SetEvent(UIEventType.Click, ExitPopup);
        _btnTestArmor.gameObject.SetEvent(UIEventType.Click, ChangePopArmor);
        _btnTestWeapon.gameObject.SetEvent(UIEventType.Click, ChangePopWeapon);
        _BtnSelectEquip.gameObject.SetEvent(UIEventType.Click, EquipmentSelectItem);
        _btnSelectReinforce.gameObject.SetEvent(UIEventType.Click, ReinforceSelectItem);
        _btnWeaponTypeReinforce.gameObject.SetEvent(UIEventType.Click, ReinforceWeaponTypeItem);
        _btnArmorTypeReinforce.gameObject.SetEvent(UIEventType.Click, ReinforceArmorTypeItem);
    }

    #endregion

    #region OtherMethod

    //강화에 필요한아이템 개수를 계산합니다.
    private void OperNeedItemCount()
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
    public void SetSelectItemInfo(InventorySlotData selectItemData)
    {
        //UI 정보를 세팅합니다.
        _selectItemData = selectItemData;
        _itemNameText.text = Manager.Inventory.ItemDataDictionary[selectItemData.itemID].itemName;
        _rarityText.text = Manager.Inventory.ItemDataDictionary[selectItemData.itemID].rarity;
        _itemLevelText.text = _selectItemData.level.ToString();

        _itemImage.sprite = Manager.Resource.GetSprite(_selectItemData.itemID.ToString());
        _typeIcon.sprite = Manager.Resource.GetSprite(Manager.Inventory.ItemDataDictionary[selectItemData.itemID].type);

        OperNeedItemCount();
        _itemHasCount.text = $"{_selectItemData.hasCount} / {_needCount}";
        _reinforceProgress.fillAmount = (float)_selectItemData.hasCount / _needCount;


        if (Manager.Inventory.ItemDataDictionary[selectItemData.itemID].statType == "attack")
        {
            _equipEffect.text = $"공격력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].equipStat + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].reinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"공격력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].retentionEffect + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].reinforceEffect * _selectItemData.level}%";
        }

        else if(Manager.Inventory.ItemDataDictionary[selectItemData.itemID].statType == "hp")
        {
            _equipEffect.text = $"체력 : {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].equipStat + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].reinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"체력 :  {Manager.Inventory.ItemDataDictionary[selectItemData.itemID].retentionEffect + Manager.Inventory.ItemDataDictionary[selectItemData.itemID].reinforceEffect * _selectItemData.level}%";
        }

        if (_selectItemData.level == 1 && _selectItemData.hasCount == 0)
        {
            _BtnSelectEquip.interactable = false;
            _btnSelectReinforce.interactable= false;
        }
        else
        {
            _BtnSelectEquip.interactable = true;
            _btnSelectReinforce.interactable = true;
        }
    }

    //선택한 아이템을 착용합니다.
    private void EquipmentSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ChangeEquipmentItem(_selectItemData);
        Manager.NotificateDot.SetEquipmentNoti();

        Manager.NotificateDot.SetRecommendWeaponNoti();
        Manager.NotificateDot.SetRecommendArmorNoti();

        Manager.NotificateDot.SetWeaponEquipmentNoti();
        Manager.NotificateDot.SetArmorEquipmentNoti();

        CallEquipRefreshEvent();
    }
    private void CallEquipRefreshEvent()
    {
        refreshReinforecEvent?.Invoke();
    }

    //선택한 아이템을 강화합니다.
    private void ReinforceSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectItem(_selectItemData);

        Manager.NotificateDot.SetReinforceWeaponNoti();
        Manager.NotificateDot.SetReinforceArmorNoti();

        Manager.NotificateDot.SetRecommendWeaponNoti();
        Manager.NotificateDot.SetRecommendArmorNoti();

        Manager.NotificateDot.SetWeaponEquipmentNoti();
        Manager.NotificateDot.SetArmorEquipmentNoti();

        Manager.NotificateDot.SetEquipmentNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    //무기 종류 일괄 강화
    private void ReinforceWeaponTypeItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(Manager.Inventory.WeaponItemList);
        //이후 각각 조건에 맞춰 버튼에 알람이 활성화 되어야 하는지, 종료되어야 하는지에 대한 정보를 뿌립니다.
        Manager.NotificateDot.SetRecommendWeaponNoti();
        Manager.NotificateDot.SetWeaponEquipmentNoti();
        Manager.NotificateDot.SetReinforceWeaponNoti();
        Manager.NotificateDot.SetEquipmentNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    //방어구 종류 일괄 강화
    private void ReinforceArmorTypeItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(Manager.Inventory.ArmorItemList);
        //이후 각각 조건에 맞춰 버튼에 알람이 활성화 되어야 하는지, 종료되어야 하는지에 대한 정보를 뿌립니다.
        Manager.NotificateDot.SetRecommendArmorNoti();
        Manager.NotificateDot.SetArmorEquipmentNoti();
        Manager.NotificateDot.SetReinforceArmorNoti();
        Manager.NotificateDot.SetEquipmentNoti();
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    private void CallReinforceRefreshEvent()
    {
        refreshReinforecEvent?.Invoke();
    }


    //EquipFillterType 상태에 맞춰 보여주는 장비류 필터를 Inventory Manager로부터 가져옵니다.
    private void FillterCurrentPopupUseItemData()
    {
        if (equipFillterType == EquipFillterType.Weapon)
        {
            _fillterItems = Manager.Inventory.WeaponItemList;
        }
        else if(equipFillterType == EquipFillterType.Armor)
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
        if (equipFillterType == EquipFillterType.Weapon)
        {
            _PopupTitle.text = "무기";
            _btnWeaponTypeReinforce.gameObject.SetActive(true);
            _btnArmorTypeReinforce.gameObject.SetActive(false);
        }
        else if (equipFillterType == EquipFillterType.Armor)
        {
            _PopupTitle.text = "방어구";
            _btnWeaponTypeReinforce.gameObject.SetActive(false);
            _btnArmorTypeReinforce.gameObject.SetActive(true);
        }
    }

    //무기 팝업으로 변경
    private void ChangePopWeapon(PointerEventData enterEvent)
    {
        equipFillterType = EquipFillterType.Weapon;
        FillterCurrentPopupUseItemData();
        SetItemTypeUI();
        SetFirstVisibleItem();

        _itemContainer.gameObject.GetComponent<UIPopupEquipContainer>().InitSlot();
    }

    //방어구 팝업으로 변경
    private void ChangePopArmor(PointerEventData enterEvent)
    {
        equipFillterType = EquipFillterType.Armor;
        FillterCurrentPopupUseItemData();
        SetItemTypeUI();
        SetFirstVisibleItem();

        _itemContainer.gameObject.GetComponent<UIPopupEquipContainer>().InitSlot();
    }


    // 팝업 닫기
    private void ExitPopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    #endregion

    #region Unity Flow

    private void OnDestroy()
    {
        refreshReinforecEvent = null;
    }

    #endregion
}