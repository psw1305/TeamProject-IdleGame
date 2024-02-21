using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerSlotsEquip : MonoBehaviour
{
    private UserInvenFollowerData _userInvenFollowerData;

    //[SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image FollowerIcon;
    private Image _bgImg;
    private Button _btn;
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
        // TODO => FollowerContainer에서 가져오도록
        //FollowerIcon.sprite = Manager.Asset.GetSprite(userInvenFollowerData.itemID);
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

        var instancePopup = Manager.UI.ShowPopup<UIPopupFollowerDetail>();
        instancePopup.SetFollowerData(_userInvenFollowerData);
    }
}
