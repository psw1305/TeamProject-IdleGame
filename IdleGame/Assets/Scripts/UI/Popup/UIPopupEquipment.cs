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

    private Button _equipBtn;
    private Button _reinforceBtn;

    private Button _btnExit;

    private ItemData _selectItemData;

    private List<ItemData> _fillterItems;

    #endregion

    #region Properties

    public event Action refreshEvent;

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
        _equipBtn = GetUI<Button>("Btn_Equip");
        _reinforceBtn = GetUI<Button>("Btn_Reinforce");
    }

    private void SetEvents()
    {
        _btnExit.gameObject.SetEvent(UIEventType.Click, ExitPopup);
        _equipBtn.gameObject.SetEvent(UIEventType.Click, EquipmentSelectItem);
    }

    #endregion

    #region OtherMethod

    // 선택한 아이템의 정보를 상단 UI에 설정하는 메서드입니다.
    public void SetSelectItemInfo(ItemData selectItemData)
    {
        //UI 정보를 세팅합니다.
        _selectItemData = selectItemData;
        _itemNameText.text = _selectItemData.itemName;
        _rarityText.text = _selectItemData.rarity;
        _itemLevelText.text = _selectItemData.level.ToString();
        _itemHasCount.text = $"{_selectItemData.hasCount} / 15";
        _itemImage.sprite = Manager.Resource.GetSprite(_selectItemData.itemID.ToString());
        _typeIcon.sprite = Manager.Resource.GetSprite(_selectItemData.type);

        _reinforceProgress.fillAmount = (float)_selectItemData.hasCount / 15;

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
            _equipBtn.interactable = false;
            _reinforceBtn.interactable= false;
        }
        else
        {
            _equipBtn.interactable = true;
            _reinforceBtn.interactable = true;
        }
    }

    //선택한 아이템을 착용합니다.
    private void EquipmentSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ChangeItem(_selectItemData);
        CallRefreshEvent();
    }

    private void CallRefreshEvent()
    {
        refreshEvent?.Invoke();
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


    // 팝업 닫기
    private void ExitPopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    #endregion

    #region Unity Flow

    private void OnDestroy()
    {
        refreshEvent = null;
    }

    #endregion
}