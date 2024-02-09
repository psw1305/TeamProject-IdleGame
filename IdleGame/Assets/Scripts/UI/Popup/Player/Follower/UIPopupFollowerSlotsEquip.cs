using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerSlotsEquip : MonoBehaviour
{
    private UserInvenFollowerData _userInvenFollowerData;

    //[SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image FollowerIcon;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupFollowerDetailInfo);
    }

    public void SetSlotUI(UserInvenFollowerData userInvenFollowerData)
    {
        _userInvenFollowerData = userInvenFollowerData;
        //levelText.text = $"Lv.{userInvenFollowerData.level}";

        FollowerIcon.gameObject.SetActive(true);
        FollowerIcon.sprite = Manager.Assets.GetSpriteFollower(userInvenFollowerData.itemID);
    }

    public void SetSlotEmpty()
    {
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
