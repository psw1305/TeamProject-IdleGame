using System.Linq;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupSkillDetail : UIPopup
{
    #region Field & Properties
    private int _needCount;

    private Image _IconSprite;
    private Image _bgImg;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _rarityText;

    private TextMeshProUGUI _descriptionText;
    private TextMeshProUGUI _coolTimeText;
    private TextMeshProUGUI _levelText;

    private TextMeshProUGUI _reinforceProgressText;
    private Image _reinforceProgressSprite;

    private TextMeshProUGUI _retentionEffectText;

    private Button _equipBtn;
    private Button _unequipBtn;
    private Button _reinforceBtn;

    private UserInvenSkillData _data;

    #endregion


    protected override void Init()
    {
        base.Init();

        SetImage();
        SetTextMeshProUGUI();

        SetButtonEvents();
        SetEquipBtn();
        SetReinforceBtn();

        SetUISkillData();
    }

    public void SetSkillData(UserInvenSkillData userInvenSkillData)
    {
        _data = userInvenSkillData;
    }

    private void SetImage()
    {
        SetUI<Image>();

        _IconSprite = GetUI<Image>("Img_Skill_Icon");
        _bgImg = GetUI<Image>("Img_Skill_Icon_BG");
        _reinforceProgressSprite = GetUI<Image>("Img_ReinforceProgress");
    }

    private void SetTextMeshProUGUI()
    {
        SetUI<TextMeshProUGUI>();

        _nameText = GetUI<TextMeshProUGUI>("Text_SkillName");
        _rarityText = GetUI<TextMeshProUGUI>("Text_Rarity");
        _levelText = GetUI<TextMeshProUGUI>("Text_Lv");
        _reinforceProgressText = GetUI<TextMeshProUGUI>("Text_hasCount");
        _descriptionText = GetUI<TextMeshProUGUI>("Text_Description");
        _coolTimeText = GetUI<TextMeshProUGUI>("Text_CoolTime");
        _retentionEffectText = GetUI<TextMeshProUGUI>("Text_RetentionStat");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();

        _equipBtn = SetButtonEvent("Btn_Equip", UIEventType.Click, EquipSkill);
        _unequipBtn = SetButtonEvent("Btn_Unequip", UIEventType.Click, UnequipSkill);
        _reinforceBtn = SetButtonEvent("Btn_Reinforce", UIEventType.Click, ReinforceSkill);

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    private void SetUISkillData()
    {
        _bgImg.color = Utilities.SetSlotTierColor(Manager.SkillData.SkillDataDictionary[_data.itemID].Rarity);
        _IconSprite.sprite = Manager.SkillData.SkillDataDictionary[_data.itemID].Sprite;

        _rarityText.color = Utilities.SetSlotTierColor(Manager.SkillData.SkillDataDictionary[_data.itemID].Rarity);
        _rarityText.text = Manager.SkillData.SkillDataDictionary[_data.itemID].Rarity.ToString();

        _nameText.text = Manager.SkillData.SkillDataDictionary[_data.itemID].SkillName;
        SetUIReinforce();
    }

    private int CalculateReinforceNeedCount()
    {
        return _needCount = _data.level < 15 ? _data.level + 1 : 15;
    }

    private void SetUIReinforce()
    {
        _levelText.text = $"Lv. {_data.level}";

        CalculateReinforceNeedCount();
        _reinforceProgressText.text = $"{_data.hasCount} / {_needCount}";
        _reinforceProgressSprite.fillAmount = (float)_data.hasCount / _needCount;

        _descriptionText.text = 
            $"<color=red>{Manager.SkillData.SkillDataDictionary[_data.itemID].SkillDamage + Manager.SkillData.SkillDataDictionary[_data.itemID].ReinforceDamage * (_data.level - 1)}%</color>{Manager.SkillData.SkillDataDictionary[_data.itemID].Description}";

        _coolTimeText.text = $"쿨타임 : {Manager.SkillData.SkillDataDictionary[_data.itemID].SkillObject.GetComponent<BaseSkill>().CoolDown}초";

        _retentionEffectText.text = 
            $"공격력 + {Manager.SkillData.SkillDataDictionary[_data.itemID].RetentionEffect + Manager.SkillData.SkillDataDictionary[_data.itemID].ReinforceEffect * (_data.level - 1)}% ";
    }

    private void SetReinforceBtn()
    {
        if (_data.itemID == Manager.Data.SkillInvenList.Last().itemID & _data.level >= 100)
        {
            _reinforceBtn.interactable = false;
        }
        else
        {
            _reinforceBtn.interactable = _data.hasCount >= CalculateReinforceNeedCount();
        }
    }

    private void SetEquipBtn()
    {
        if (_data.equipped == true)
        {
            _equipBtn.gameObject.SetActive(false);
            _unequipBtn.gameObject.SetActive(true);
        }
        else
        {
            _equipBtn.gameObject.SetActive(true);
            _unequipBtn.gameObject.SetActive(false);
        }
    }

    private void EquipSkill(PointerEventData eventData)
    {
        SetEquipBtn();

        Manager.SkillData.CallSetUISkillEquipSlot(Manager.SkillData.EquipSkill(_data));
        Manager.SkillData.CallSetUISkillInvenSlot(_data.itemID);
        Manager.UI.ClosePopup();
    }

    private void UnequipSkill(PointerEventData eventData)
    {
        SetEquipBtn();

        Manager.SkillData.CallSetUISkillEquipSlot(Manager.SkillData.UnEquipSkill(_data));
        Manager.SkillData.CallSetUISkillInvenSlot(_data.itemID);
        Manager.UI.ClosePopup();
    }
    private void ReinforceSkill(PointerEventData eventData)
    {
        Manager.SkillData.ReinforceSelectSkill(_data);

        Manager.Notificate.SetReinforceSkillNoti();
        Manager.Notificate.SetPlayerStateNoti();

        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();

        SetReinforceBtn();
        SetUIReinforce();
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
