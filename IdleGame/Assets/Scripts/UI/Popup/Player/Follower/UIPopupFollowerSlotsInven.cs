using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupFollowerSlotsInven : MonoBehaviour
{
    #region Value Fields
    private int _needCount;
    private ItemTier _rarity;

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

        _rarity = Manager.FollowerData.FollowerDataDictionary[itemData.itemID].Rarity;
        GetComponent<Image>().color = Utilities.SetSlotTierColor(_rarity);
    }

    public void InitSlotUI()
    {
        itemSprite.sprite = Manager.FollowerData.FollowerDataDictionary[_followerData.itemID].Sprite;
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
        if (_followerData.itemID == Manager.Data.FollowerInvenList.Last().itemID & _followerData.level >= 100)
        {
            reinforceIcon.SetActive(false);
        }
        else if (_followerData.hasCount < _needCount)
        {
            reinforceIcon.SetActive(false);
        }
        else
        {
            reinforceIcon.SetActive(true);
        }
    }


    public void SetUILockState()
    {
        if (_followerData.level > 1 || _followerData.hasCount > 0)
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
