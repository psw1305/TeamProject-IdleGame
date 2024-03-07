using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerSlotsEquip : MonoBehaviour
{
    private UserInvenFollowerData _userInvenFollowerData;
    [SerializeField] private Image FollowerIcon;
    private Image _bgImg;
    private Button _btn;
    public bool ReplaceMode = false;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupFollowerDetailInfo);
        _bgImg = GetComponent<Image>();
        _btn = GetComponent<Button>();
    }

    public void SetSlotUI(UserInvenFollowerData userInvenFollowerData)
    {
        _btn.interactable = true;
        _bgImg.color = Utilities.SetSlotTierColor(Manager.FollowerData.FollowerDataDictionary[userInvenFollowerData.itemID].Rarity);
        _userInvenFollowerData = userInvenFollowerData;
        //levelText.text = $"Lv.{userInvenFollowerData.level}";

        FollowerIcon.gameObject.SetActive(true);
        FollowerIcon.sprite = Manager.FollowerData.FollowerDataDictionary[userInvenFollowerData.itemID].Sprite;
    }

    public void SetSlotEmpty()
    {
        _btn.interactable = false;
        _bgImg.color = Color.white;
        //levelText.text = string.Empty;
        FollowerIcon.gameObject.SetActive(false);
    }

    private void ShowPopupFollowerDetailInfo()
    {
        if (_userInvenFollowerData == null)
        {
            return;
        }
        
        if(ReplaceMode == false)
        {
        var instancePopup = Manager.UI.ShowPopup<UIPopupFollowerDetail>();
        instancePopup.SetFollowerData(_userInvenFollowerData);
        }
        else
        {
            Manager.FollowerData.UnEquipFollower(_userInvenFollowerData);
            Manager.FollowerData.CallSetUIFollowerInvenSlot(_userInvenFollowerData.itemID);

            Manager.FollowerData.CallSetUIFollowerEquipSlot(Manager.FollowerData.EquipFollower(Manager.FollowerData.ReplaceFollower));
            Manager.FollowerData.CallSetUIFollowerInvenSlot(Manager.FollowerData.ReplaceFollower.itemID);
            transform.parent.GetComponent<UIPopupFollowerContainerEquip>().ToggleSlotReplaceMode();
        }
    }
}
