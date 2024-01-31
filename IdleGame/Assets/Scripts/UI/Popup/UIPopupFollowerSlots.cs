using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerSlots : MonoBehaviour
{
    #region Value Fields

    private string _itemID;
    private string _followerName;
    private string _rarity;
    private int _level;
    private int _hasCount;
    private int _needCount;
    private int _damageCorrection;
    //private float _equipStat;
    private float _reinforceEquip;
    private float _reinforceDamage;
    private float _retentionEffect;
    private float _reinforceEffect;
    private bool _equipped;

    #endregion

    #region Properties

    public string ItemID => _itemID;
    public string FollowerName => _followerName;
    public string Rarity => _rarity;
    public int Level => _level;
    public int HasCount => _hasCount;
    public int DamageCorrection => _damageCorrection;
    //public float EquipStat => _equipStat + _reinforceEquip;
    public float RetentionEffect => _retentionEffect + _reinforceEffect;
    public bool Equipped => _equipped;

    #endregion

    #region Object Fields

    public Action SetReinforceUI;

    [SerializeField] private TextMeshProUGUI _lvTxt;
    [SerializeField] private GameObject _equippdText;

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private GameObject ReinforceIcon;

    private UserInvenFollowerData _followerData;
    public UserInvenFollowerData ItemData => _followerData;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        //SetReinforceUI += SetReinforceData;
        //SetReinforceUI += SetReinforceProgress;
        //SetReinforceUI += SetReinforceIcon;
    }

    #endregion

    #region Init
    public void InitSlotInfo(UserInvenFollowerData itemData)
    {
        _followerData = itemData;
        _itemID = _followerData.itemID;
        _followerName = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].followerName;
        _level = _followerData.level;
        _rarity = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].rarity;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _followerData.hasCount;
        //_equipStat = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].equipStat;
        //_reinforceEquip = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].reinforceEquip * _level;
        _retentionEffect = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].retentionEffect;
        _reinforceEffect = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].reinforceEffect * _level;
        _equipped = _followerData.equipped;
    }

    public void InitSlotUI()
    {
        _lvTxt.text = $"Lv : {_level}";

        itemSprite.sprite = Manager.Resource.GetSprite(ItemID.ToString());

        //SetLockState();
        //gameObject.GetComponent<Button>().onClick.AddListener(SendItemData);
    }

    #endregion
}
