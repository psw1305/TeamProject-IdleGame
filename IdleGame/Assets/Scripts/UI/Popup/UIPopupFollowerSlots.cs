using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerSlots : MonoBehaviour
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

    [SerializeField] private TextMeshProUGUI _lvTxt;
    [SerializeField] private GameObject _equippdText;

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private GameObject ReinforceIcon;

    private UserInvenFollowerData _followerData;
    public UserInvenFollowerData FollowerData => _followerData;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetReinforceUI += SetReinforceData;
        SetReinforceUI += SetReinforceProgress;
        SetReinforceUI += SetReinforceIcon;
    }

    private void OnDestroy()
    {
        SetReinforceUI -= SetReinforceData;
        SetReinforceUI -= SetReinforceProgress;
        SetReinforceUI -= SetReinforceIcon;
    }

    #endregion

    #region Init
    public void InitSlotInfo(UserInvenFollowerData itemData)
    {
        _followerData = itemData;
        _itemID = _followerData.itemID;
        _level = _followerData.level;
        _rarity = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].rarity;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _followerData.hasCount;
    }

    public void InitSlotUI()
    {
        _lvTxt.text = $"Lv : {_level}";

        itemSprite.sprite = Manager.Resource.GetSprite(ItemID.ToString());

        SetLockState();
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupFollowerDetailInfo);
    }

    #endregion

    public void CheckEquipState()
    {
        if (_followerData.equipped == false)
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
        _level = _followerData.level;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _followerData.hasCount;
        if (_followerData.level < 15)
        {
            _needCount = _followerData.level + 1;
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
        if (FollowerData.hasCount >= _needCount)
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

    private void ShowPopupFollowerDetailInfo()
    {
        var instancePopup = Manager.UI.ShowPopup<UIPopupFollowerDetail>();
        instancePopup.SetFollowerData(_followerData);
    }
}
