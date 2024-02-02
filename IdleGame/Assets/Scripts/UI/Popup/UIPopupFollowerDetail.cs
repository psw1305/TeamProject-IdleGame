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

    private TextMeshProUGUI _atkDamageText;
    private TextMeshProUGUI _atkSpeedText;
    private TextMeshProUGUI _retentionEffectText;

    private Button btn_Equip;
    private Button btn_UnEquip;

    private UIPopupFollower _popupFollower;

    private int _needCount;

    protected override void Init()
    {
        base.Init();

        SetImage();
        SetTextMeshProUGUI();
        SetButtonEvents();
        
        SetUIFollowerData();
        SetEquipTypeUI();
    }
    private void SetImage()
    {
        SetUI<Image>();

        _IconSprite = GetUI<Image>("Img_Follower_Icon");
        _reinforceProgressSprite = GetUI<Image>("Img_ReinforceProgress");
    }
    private void SetTextMeshProUGUI()
    {
        SetUI<TextMeshProUGUI>();

        _nameText = GetUI<TextMeshProUGUI>("Text_FollowerName");
        _rarityText = GetUI<TextMeshProUGUI>("Text_Rarity");
        _levelText = GetUI<TextMeshProUGUI>("Text_Lv");

        _atkDamageText = GetUI<TextMeshProUGUI>("Text_AtkStat");
        _atkSpeedText = GetUI<TextMeshProUGUI>("Text_AtkStpeedStat");

        _reinforceProgressText = GetUI<TextMeshProUGUI>("Text_hasCount");
        _retentionEffectText = GetUI<TextMeshProUGUI>("Text_RetentionStat");
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();

        btn_Equip = SetButtonEvent("Btn_Equip", UIEventType.Click, EquipFollower);
        btn_UnEquip = SetButtonEvent("Btn_UnEquip", UIEventType.Click, UnEquipFollower);

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    public void SetUIFollowerData()
    {
        SetReinforceData();

        _IconSprite.sprite = Manager.Resource.GetSprite(_data.itemID);

        _nameText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].followerName;
        _rarityText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].rarity;

        _levelText.text = $"Lv.{_data.level}";

        _atkDamageText.text = (Manager.Game.Player.AtkDamage.Value * Manager.FollowerData.FollowerDataDictionary[_data.itemID].damageCorrection / 100).ToString();
        _atkSpeedText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].atkSpeed.ToString();

        _reinforceProgressText.text = $"{_data.hasCount} / {_needCount}";
        _reinforceProgressSprite.fillAmount = (float)_data.hasCount / _needCount; ;

        _retentionEffectText.text = $"공격력 +{Manager.Game.Player.AtkDamage.Value * ((Manager.FollowerData.FollowerDataDictionary[_data.itemID].retentionEffect + Manager.FollowerData.FollowerDataDictionary[_data.itemID].reinforceEffect*_data.level) / 100)}%";
    }

    public void SetFollowerData(UserInvenFollowerData userInvenFollowerData)
    {
        _data = userInvenFollowerData;
    }

    private void SetReinforceData()
    {
        if (_data.level < 15)
        {
            _needCount = _data.level + 1;
        }
        else
        {
            _needCount = 15;
        }
    }

    private void EquipFollower(PointerEventData enterEvent)
    {
        _data.equipped = true;
        SetEquipTypeUI();
    }

    private void UnEquipFollower(PointerEventData enterEvent)
    {
        _data.equipped = false;
        SetEquipTypeUI();
    }

    private void SetEquipTypeUI()
    {
        if (_data.equipped)
        {
            btn_UnEquip.gameObject.SetActive(true);
            btn_Equip.gameObject.SetActive(false);
        }
        else
        {
            btn_Equip.gameObject.SetActive(true);
            btn_UnEquip.gameObject.SetActive(false);
        }
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
