using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerSlotsInven : MonoBehaviour
{
    #region Value Fields

    private string _itemID;
    private int _level;
    private int _hasCount;
    private int _needCount;
    private string _rarity;

    #endregion

    #region Properties

    public string ItemID => _itemID;

    #endregion

    #region Object Fields

    public Action SetReinforceUI;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject equippdText;

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private GameObject reinforceIcon;

    private UserInvenFollowerData _followerData;
    public UserInvenFollowerData FollowerData => _followerData;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetReinforceUI += SetReinforceData;
        SetReinforceUI += SetUIReinforceIcon;
    }

    private void OnDestroy()
    {
        SetReinforceUI -= SetReinforceData;
        SetReinforceUI -= SetUIReinforceIcon;
    }

    #endregion

    #region Init
    public void InitSlotInfo(UserInvenFollowerData itemData)
    {
        _followerData = itemData;
        _itemID = _followerData.itemID;
        _level = _followerData.level;
        _hasCount = _followerData.hasCount;
        _rarity = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].Rarity;
    }

    public void InitSlotUI()
    {
        //itemSprite.sprite = Manager.Asset.GetSprite(ItemID.ToString());
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupFollowerDetailInfo);

        SetUILockState();
    }

    #endregion

    public void SetReinforceData()
    {
        levelText.text = $"Lv. {_followerData.level}";
        _needCount = _followerData.level < 15 ? _followerData.level + 1 : 15;
        reinforceText.text = $"{_followerData.hasCount} / {_needCount}";
        reinforceProgress.fillAmount = (float)_followerData.hasCount / _needCount;
    }

    public void SetUIEquipState()
    {
        if (_followerData.equipped == false)
        {
            equippdText.SetActive(false);
        }
        else
        {
            equippdText.SetActive(true);
        }
    }

    public void SetUIReinforceIcon()
    {
        if (FollowerData.hasCount >= _needCount)
        {
            reinforceIcon.SetActive(true);
        }
        else
        {
            reinforceIcon.SetActive(false);
        }
    }    


    public void SetUILockState()
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

    private void ShowPopupFollowerDetailInfo()
    {
        var instancePopup = Manager.UI.ShowPopup<UIPopupFollowerDetail>();
        instancePopup.SetFollowerData(_followerData);
    }
}
