using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerDetail : UIPopup
{
    private UserInvenFollowerData _data;
    public UserInvenFollowerData Data => _data;

    private Image _IconSprite;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _rarityText;

    private TextMeshProUGUI _levelText;

    private TextMeshProUGUI _reinforceProgressText;
    private Image _reinforceProgressSprite;

    private TextMeshProUGUI _retentionEffectText;

    protected override void Init()
    {
        base.Init();

        SetImage();
        SetTextMeshProUGUI();

        SetButtonEvents();

        //SetUIFollowerData();
    }
    private void SetImage()
    {
        SetUI<Image>();

        _IconSprite = GetUI<Image>("Img_Follower_Icon");
    }
    private void SetTextMeshProUGUI()
    {
        _nameText = GetUI<TextMeshProUGUI>("Text_FollowerName");
        _rarityText = GetUI<TextMeshProUGUI>("Text_Rarity");
        _levelText = GetUI<TextMeshProUGUI>("Text_Lv");
        _reinforceProgressText = GetUI<TextMeshProUGUI>("Text_hasCount");
        _retentionEffectText = GetUI<TextMeshProUGUI>("Text_RetentionStat");
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    public void SetUIFollowerData()
    {
        _IconSprite.sprite = Manager.Resource.GetSprite(_data.itemID);

        _nameText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].followerName;
        _rarityText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].rarity;

        _levelText.text = _data.level.ToString();

        _reinforceProgressText.text = "";
        _reinforceProgressSprite.fillAmount = 1;

        _retentionEffectText.text = "dummy";
    }

    public void SetFollowerData(UserInvenFollowerData userInvenFollowerData)
    {
        _data = userInvenFollowerData;
    }


    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
