using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupSkillDetail : UIPopup
{
    private UserInvenSkillData _data;
    public UserInvenSkillData Data => _data;

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

        //SetUISkillData();
    }
    private void SetImage()
    {
        SetUI<Image>();

        _IconSprite = GetUI<Image>("Img_Skill_Icon");
    }
    private void SetTextMeshProUGUI()
    {
        _nameText = GetUI<TextMeshProUGUI>("Text_SkillName");
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

    public void SetUISkillData()
    {
        _IconSprite.sprite = Manager.Resource.GetSprite(_data.itemID);

        _nameText.text = Manager.SkillData.SkillDataDictionary[_data.itemID].skillName;
        _rarityText.text = Manager.SkillData.SkillDataDictionary[_data.itemID].rarity;

        _levelText.text = _data.level.ToString();

        _reinforceProgressText.text = "";
        _reinforceProgressSprite.fillAmount = 1;

        _retentionEffectText.text = "dummy";
    }

    public void SetSkillData(UserInvenSkillData userInvenSkillData)
    {
        _data = userInvenSkillData;
    }


    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
