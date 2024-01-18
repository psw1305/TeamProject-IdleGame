using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using System.Collections.Generic;

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
    private Button _btnSameTypeReinforce;

    private Button _btnTestWeapon;
    private Button _btnTestArmor;

    private Image _itemContainer;

    private ItemData _selectItemData;

    private List<ItemData> _fillterItems;

    private int _needCount;
    #endregion

    #region Properties

    public event Action refreshEquipEvent;

    public event Action refreshReinforecEvent;

    public EquipFillterType equipFillterType;

    public List<ItemData> FillterItems => _fillterItems;

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
        SetPopupTitle();
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
        _btnSameTypeReinforce = GetUI<Button>("Btn_ReinforceAll");
        _btnTestWeapon = GetUI<Button>("Btn_TestWeapon");
        _btnTestArmor = GetUI<Button>("Btn_TestArmor");
    }

    private void SetEvents()
    {
        _btnExit.gameObject.SetEvent(UIEventType.Click, ExitPopup);
        _BtnSelectEquip.gameObject.SetEvent(UIEventType.Click, EquipmentSelectItem);
        _btnSelectReinforce.gameObject.SetEvent(UIEventType.Click, ReinforceSelectItem);
        _btnSameTypeReinforce.gameObject.SetEvent(UIEventType.Click, ReinforceTypeItem);
        _btnTestWeapon.gameObject.SetEvent(UIEventType.Click, ChangePopWeapon);
        _btnTestArmor.gameObject.SetEvent(UIEventType.Click, ChangePopArmor);
    }

    #endregion

    #region OtherMethod

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
    public void SetSelectItemInfo(ItemData selectItemData)
    {
        //UI 정보를 세팅합니다.
        _selectItemData = selectItemData;
        _itemNameText.text = _selectItemData.itemName;
        _rarityText.text = _selectItemData.rarity;
        _itemLevelText.text = _selectItemData.level.ToString();

        _itemImage.sprite = Manager.Resource.GetSprite(_selectItemData.itemID.ToString());
        _typeIcon.sprite = Manager.Resource.GetSprite(_selectItemData.type);

        OperNeedItemCount();
        _itemHasCount.text = $"{_selectItemData.hasCount} / {_needCount}";
        _reinforceProgress.fillAmount = (float)_selectItemData.hasCount / _needCount;


        if (selectItemData.type == "weapon")
        {
            _equipEffect.text = $"공격력 : {_selectItemData.equipStat + _selectItemData.reinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"공격력 : {_selectItemData.retentionEffect + _selectItemData.reinforceEffect * _selectItemData.level}%";
        }
        else
        {
            _equipEffect.text = $"체력 : {_selectItemData.equipStat + _selectItemData.reinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"체력 :  {_selectItemData.retentionEffect + _selectItemData.reinforceEffect * _selectItemData.level}%";
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
        Manager.Inventory.ChangeItem(_selectItemData);
        CallEquipRefreshEvent();
    }
    private void CallEquipRefreshEvent()
    {
        refreshReinforecEvent?.Invoke();
    }

    private void ReinforceSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectItem(_selectItemData);
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }
    private void ReinforceTypeItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ReinforceSelectTypeItem(_selectItemData);
        SetSelectItemInfo(_selectItemData);
        CallReinforceRefreshEvent();
    }

    private void CallReinforceRefreshEvent()
    {
        refreshReinforecEvent?.Invoke();
    }


    //EquipFillterType 상태에 맞춰 보여주는 무기류를 필터합니다.
    private void FillterCurrentPopupUseItemData()
    {
        if (equipFillterType == EquipFillterType.Weapon)
        {
            _fillterItems = Manager.Inventory.ItemDataBase.ItemDB.Where(item => item.type == "weapon").ToList();
        }
        else if(equipFillterType == EquipFillterType.Armor)
         {
            _fillterItems = Manager.Inventory.ItemDataBase.ItemDB.Where(item => item.type == "armor").ToList();
        }
    }
    private void SetFirstVisibleItem()
    {
        if (_fillterItems.Where(item => item.equipped == true).ToList().Count == 0)
        {
            SetSelectItemInfo(_fillterItems[0]);
        }
        else
        {
            SetSelectItemInfo(_fillterItems.Where(item => item.equipped == true).ToList()[0]);
        }
    }

    private void SetPopupTitle()
    {
        if (equipFillterType == EquipFillterType.Weapon)
        {
            _PopupTitle.text = "무기";
        }
        else if (equipFillterType == EquipFillterType.Armor)
        {
            _PopupTitle.text = "방어구";
        }
    }
    private void ChangePopWeapon(PointerEventData enterEvent)
    {
        equipFillterType = EquipFillterType.Weapon;
        FillterCurrentPopupUseItemData();
        SetPopupTitle();
        SetFirstVisibleItem();
        _itemContainer.gameObject.GetComponent<UIPopupEquipContainer>().InitSlot();
    }
    private void ChangePopArmor(PointerEventData enterEvent)
    {
        equipFillterType = EquipFillterType.Armor;
        FillterCurrentPopupUseItemData();
        SetPopupTitle();
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