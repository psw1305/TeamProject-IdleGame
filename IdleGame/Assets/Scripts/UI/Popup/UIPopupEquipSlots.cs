using System;
using TMPro;
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
    private int _needCount;
    private float _equipStat;
    private float _reinforceEquip;
    private float _retentionEffect;
    private float _reinforceEffect;
    private bool _equipped;

    public int ItemID => _itemID;
    public string ItemName => _itemName;
    public string Type => _type;
    public string Rarity => _rarity;
    public int Level => _level;
    public int HasCount => _hasCount;
    public float EquipStat => _equipStat + _reinforceEquip;
    public float RetentionEffect => _retentionEffect + _reinforceEffect;
    public bool Equipped => _equipped;


    //타 클래스에서 여러 메서드를 실행할때 콜백으로 하거나 여러번 GetCompornent 처리하는게 마음에 들지 않아 일단 Action으로 묶어두었음 
    public Action SetReinforceUI;

    [SerializeField] private TextMeshProUGUI _lvTxt;
    [SerializeField] private GameObject _equippdText;

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private GameObject ReinforceIcon;

    private ItemData _itemData;

    public ItemData ItemData => _itemData;

    private void Awake()
    {
        SetReinforceUI += SetReinforceData;
        SetReinforceUI += SetReinforceProgress;
        SetReinforceUI += SetReinforceIcon;
        Debug.Log("액션 추가 완료");
    }


    //아이템 아이콘 세팅, 티어 세팅, 레벨 세팅,게이지 세팅, 언록 여부 
    public void InitSlotInfo(ItemData itemData)
    {
        _itemData = itemData;
        _itemID = _itemData.itemID;
        _itemName = _itemData.itemName;
        _level = _itemData.level;
        _type = _itemData.type;
        _rarity = _itemData.rarity;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _itemData.hasCount;
        _equipStat = _itemData.equipStat;
        _reinforceEquip = _itemData.reinforceEquip * _level;
        _retentionEffect = _itemData.retentionEffect;
        _reinforceEffect = _itemData.reinforceEffect * _level;
        _equipped = _itemData.equipped;
    }

    public void InitSlotUI()
    {
        _lvTxt.text = $"Lv : {_level}";

        itemSprite.sprite = Manager.Resource.GetSprite(ItemID.ToString());

        SetLockState();
        //gameObject.SetEvent(UIEventType.Click, SendItemData);
        gameObject.GetComponent<Button>().onClick.AddListener(SendItemData);
    }

    public void CheckEquipState()
    {
        if (_itemData.equipped == false)
        {
            _equippdText.SetActive(false);
        }
        else
        {
            _equippdText.SetActive(true);
        }
    }

    private void SetReinforceData()
    {
        _level = _itemData.level;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _itemData.hasCount;
        if (_itemData.level < 15)
        {
            _needCount = _itemData.level + 1;
        }
        else
        {
            _needCount = 15;
        }
    }


    private void SetReinforceProgress()
    {
        reinforceProgress.fillAmount = (float)_hasCount / _needCount;
        reinforceText.text = $"{_hasCount}/{_needCount}";
    }


    private void SetReinforceIcon()
    {
        if (ItemData.hasCount >= _needCount)
        {
            ReinforceIcon.SetActive(true);
        }
        else
        {
            ReinforceIcon.SetActive(false);
        }
    }



    public void SetLockState()
    {
        if (_level > 1 || _hasCount > 0)
        {
            lockCover.SetActive(false);
            lockIcon.SetActive(false);
            return;
        }

        lockCover.SetActive(true);
        lockIcon.SetActive(true);
    }

    private void SendItemData()
    {
        FindObjectOfType<UIPopupEquipment>().SetSelectItemInfo(_itemData);
    }
}
