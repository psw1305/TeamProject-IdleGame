using System;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupEquipSlots : MonoBehaviour
{
    private int _itemID;
    private string _itemName;
    private string _type;
    private string _rarity;
    private int _level;
    private int _hasCount;
    private float _equipStat;
    private float _reinforceEquip;
    private float _retentionEffect;
    private float _reinforceEffect;
    private bool _equipped;

    private bool _canEquip;

    public int ItemID => _itemID;
    public string ItemName => _itemName;
    public string Type => _type;
    public string Rarity => _rarity;
    public int Level => _level;
    public int HasCount => _hasCount;
    public float EquipStat => _equipStat + _reinforceEquip;
    public float RetentionEffect => _retentionEffect + _reinforceEffect;
    public bool Equipped => _equipped;

    [SerializeField] private TextMeshProUGUI _lvTxt;
    [SerializeField] private GameObject _equippdText;

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    private ItemData _itemData;

    public ItemData ItemData => _itemData;
    //아이템 아이콘 세팅, 티어 세팅, 레벨 세팅,게이지 세팅, 언록 여부 
    public void InitSlotInfo(ItemData itemData)
    {
        _itemData = Manager.Inventory.ItemDataBase.ItemDB.Where(item => item.itemID == itemData.itemID).ToList()[0];
        _itemID = _itemData.itemID;
        _itemName = _itemData.itemName;
        _type = _itemData.type;
        _rarity = _itemData.rarity;
        _level = _itemData.level;
        _hasCount = _itemData.hasCount;
        _equipStat = _itemData.equipStat;
        _reinforceEquip = _itemData.reinforceEquip * _level;
        _retentionEffect = _itemData.retentionEffect;
        _reinforceEffect = _itemData.reinforceEffect * _level;
        _equipped = _itemData.equipped;
    }

    public void CheckEquipState()
    {

        _equipped = _itemData.equipped;
        if (_equipped == false)
        {
            _equippdText.SetActive(false);
        }
        else
        {
            _equippdText.SetActive(true);
        }
    }

    public void InitSlotUI()
    {
        _lvTxt.text = $"Lv : {_level}";

        itemSprite.sprite = Manager.Resource.GetSprite(ItemID.ToString());

        reinforceProgress.fillAmount = (float)_hasCount / 15;
        reinforceText.text = $"{_hasCount}/{15}";

        SetLockState();
        gameObject.SetEvent(UIEventType.Click, SendItemData);
    }

    public void SetLockState()
    {
        if (_level > 1 || _hasCount >= 1)
        {
            lockCover.SetActive(false);
            lockIcon.SetActive(false);
            _canEquip = true;
            return;
        }

        lockCover.SetActive(true);
        lockIcon.SetActive(true);
        _canEquip = false;
    }

    private void SendItemData(PointerEventData eventData)
    {
        FindObjectOfType<UIPopupEquipment>().SetSelectItemInfo(_itemData);
    }
}
