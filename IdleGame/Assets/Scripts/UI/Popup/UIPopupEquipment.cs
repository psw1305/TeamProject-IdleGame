using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIPopupEquipment : UIPopup
{

    private Image _itemImage;
    private Image _typeIcon;

    private TextMeshProUGUI _itemNameText;

    private TextMeshProUGUI _rarityText;
    private TextMeshProUGUI _itemLevelText;

    private TextMeshProUGUI _itemHasCount;

    private TextMeshProUGUI _equipEffect;
    private TextMeshProUGUI _retentionEffect;

    private Button _equipBtn;
    private Button _reinforceBtn;

    private Button _btnExit;

    private ItemData _selectItemData;

    private Image _slotMask;
    private Image _slotContainer;

    protected override void Init()
    {
        base.Init();
        SetText();
        SetImage();
        SetButtons();
        SetEvents();
    }

    private void SetImage()
    {
        SetUI<Image>();
        _itemImage = GetUI<Image>("Img_EquipSlot");
        _typeIcon = GetUI<Image>("Img_ETypeIcon");
        _slotMask = GetUI<Image>("Viewport");
        _slotContainer = GetUI<Image>("Itemcontainer");
    }

    private void SetText()
    {
        SetUI<TextMeshProUGUI>();
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

    // 선택한 아이템의 정보를 상단 UI에 설정하는 메서드입니다.
    public void SetSelectItemInfo(ItemData selectItemData)
    {
        //UI 정보를 세팅합니다.
        _selectItemData = selectItemData;
        _itemNameText.text = _selectItemData.itemName;
        _rarityText.text = _selectItemData.rarity;
        _itemLevelText.text = _selectItemData.level.ToString();
        _itemHasCount.text = _selectItemData.hasCount.ToString();

        if (selectItemData.type == "weapon")
        {
            _equipEffect.text = $"공격력 : {_selectItemData.equipStat + _selectItemData.reinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"공격력 : {_selectItemData.retentionEffect + _selectItemData.reinforceEffect * _selectItemData.level}%";
        }
        else
        {
            _equipEffect.text = $"방어력 : {_selectItemData.equipStat + _selectItemData.reinforceEquip * _selectItemData.level}%";
            _retentionEffect.text = $"방어력 :  {_selectItemData.retentionEffect + _selectItemData.reinforceEffect * _selectItemData.level}%";
        }
    }
    //선택한 아이템 장착
    private void EquipmentSelectItem(PointerEventData enterEvent)
    {
        Manager.Inventory.ChangeItem(_selectItemData);
    }

    // 팝업 닫기
    private void ExitPopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}