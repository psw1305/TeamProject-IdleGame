using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupFollowerDetail : UIPopup
{
    #region Fields & Properties
    private int _needCount;

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
    private Button btn_reinforce;

    private UserInvenFollowerData _data;

    #endregion

    protected override void Init()
    {
        base.Init();

        SetImage();
        SetTextMeshProUGUI();
        SetButtonEvents();
        
        SetUIFollowerData();
        SetEquipBtn();
    }

    public void SetFollowerData(UserInvenFollowerData userInvenFollowerData)
    {
        _data = userInvenFollowerData;
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
        btn_reinforce = SetButtonEvent("Btn_Reinforce", UIEventType.Click, ReinforceFollower);

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    public void SetUIFollowerData()
    {
        _IconSprite.sprite = Manager.Resource.GetSprite(_data.itemID);

        _nameText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].followerName;
        _rarityText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].rarity;

        _atkDamageText.text = (Manager.Game.Player.AtkDamage.Value * Manager.FollowerData.FollowerDataDictionary[_data.itemID].damageCorrection / 100).ToString();
        _atkSpeedText.text = Manager.FollowerData.FollowerDataDictionary[_data.itemID].atkSpeed.ToString();

        _retentionEffectText.text = $"공격력 +{Manager.Game.Player.AtkDamage.Value * ((Manager.FollowerData.FollowerDataDictionary[_data.itemID].retentionEffect + Manager.FollowerData.FollowerDataDictionary[_data.itemID].reinforceEffect*_data.level) / 100)}%";

        SetUIReinforce();
    }

    private int CalcReinforceNeedCount()
    {
        return _needCount = _data.level < 15 ? _data.level + 1 : 15;
    }

    private void SetUIReinforce()
    {
        _levelText.text = $"Lv. {_data.level}";

        CalcReinforceNeedCount();
        _reinforceProgressText.text = $"{_data.hasCount} / {_needCount}";
        _reinforceProgressSprite.fillAmount = (float)_data.hasCount / _needCount;
    }

    private void SetReinforceBtn()
    {
        btn_reinforce.interactable = _data.hasCount >= CalcReinforceNeedCount();
    }

    private void SetEquipBtn()
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

    private void EquipFollower(PointerEventData enterEvent)
    {
        SetEquipBtn();
        Manager.FollowerData.CallSetUIFollowerEquipSlot(Manager.FollowerData.EquipFollower(_data));
        Manager.FollowerData.CallSetUIFollowerInvenSlot(_data.itemID);
        Manager.UI.ClosePopup();
    }

    private void UnEquipFollower(PointerEventData enterEvent)
    {
        SetEquipBtn();

        Manager.FollowerData.CallSetUIFollowerEquipSlot(Manager.FollowerData.UnEquipFollower(_data));
        Manager.FollowerData.CallSetUIFollowerInvenSlot(_data.itemID);
        Manager.UI.ClosePopup();
    }

    private void ReinforceFollower(PointerEventData eventData)
    {
        Manager.FollowerData.ReinforceFollower(_data);
        Manager.FollowerData.CallSetUIFollowerInvenSlot(_data.itemID);

        SetReinforceBtn();
        SetUIReinforce();
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}