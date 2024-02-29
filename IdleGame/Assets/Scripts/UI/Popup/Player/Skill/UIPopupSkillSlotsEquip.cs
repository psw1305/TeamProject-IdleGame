using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkillSlotsEquip : MonoBehaviour
{
    private UserInvenSkillData _userInvenSkillData;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image skillIcon;
    private Image _bgImg;
    private Button _btn;
    public bool ReplaceMode = false;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupSkillDetailInfo);
        _bgImg = GetComponent<Image>();
        _btn = GetComponent<Button>();
    }

    public void SetSlotUI(UserInvenSkillData userInvenSkillData)
    {
        _btn.interactable = true;
        _userInvenSkillData = userInvenSkillData;
        levelText.text = $"Lv.{userInvenSkillData.level}";

        skillIcon.gameObject.SetActive(true);

        SkillBlueprint sb = Manager.SkillData.SkillDataDictionary[userInvenSkillData.itemID];
        _bgImg.color = Utilities.SetSlotTierColor(sb.Rarity);
        skillIcon.sprite = sb.Sprite;
    }

    public void SetSlotEmpty()
    {
        _btn.interactable = false;
        _bgImg.color = Color.white;
        levelText.text = string.Empty;
        skillIcon.gameObject.SetActive(false);
    }

    private void ShowPopupSkillDetailInfo()
    {
        if (_userInvenSkillData == null)
        {
            return;
        }

        if(ReplaceMode == false)
        {
            var instancePopup = Manager.UI.ShowPopup<UIPopupSkillDetail>();
            instancePopup.SetSkillData(_userInvenSkillData);
        }
        else
        {
            Manager.SkillData.UnEquipSkill(_userInvenSkillData);
            Manager.SkillData.CallSetUISkillInvenSlot(_userInvenSkillData.itemID);

            Manager.SkillData.CallSetUISkillEquipSlot(Manager.SkillData.EquipSkill(Manager.SkillData.ReplaceSkill));
            Manager.SkillData.CallSetUISkillInvenSlot(Manager.SkillData.ReplaceSkill.itemID);
            transform.parent.GetComponent<UIPopupSkillSlotContainerEquip>().ToggleSlotReplaceMode();
        }
    }
}
