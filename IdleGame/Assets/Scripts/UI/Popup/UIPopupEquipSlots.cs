using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
enum ButtonLockState
{
    UnLock,
    Lock
}
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

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    public SlotData _slotData;
    //아이템 아이콘 세팅, 티어 세팅, 레벨 세팅,게이지 세팅, 언록 여부 
    public void InitSlotInfo(SlotData data)
    {
        _slotData = data;
        _itemID = _slotData.itemID;
        _itemName = _slotData.itemName;
        _type = _slotData.type;
        _rarity = _slotData.rarity;
        _level = _slotData.level;
        _hasCount = _slotData.hasCount;
        _equipStat = _slotData.equipStat;
        _reinforceEquip = _slotData.reinforceEquip * _level;
        _retentionEffect = _slotData.retentionEffect;
        _reinforceEffect = _slotData.reinforceEffect * _level;
    }

    public void InitSlotUI()
    {
        _lvTxt.text = $"Lv : {_level}";

        itemSprite.sprite = Resources.Load<Sprite>($"Item.10001");

        reinforceProgress.fillAmount = _hasCount / 15;
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
            return;
        }

        lockCover.SetActive(true);
        lockIcon.SetActive(true);
    }

    private void SendItemData(PointerEventData eventData)
    {
        transform.parent.GetComponent<UIPopupEquipContainer>()
            .itemInfoUI.GetComponent<UIPopupEquipItemInfo>()
            .SetSelectItemInfo(_slotData);
    }
}
