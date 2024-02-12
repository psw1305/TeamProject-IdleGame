using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkillSlotsEquip : MonoBehaviour
{
    private UserInvenSkillData _userInvenSkillData;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image skillIcon;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupSkillDetailInfo);
    }

    public void SetSlotUI(UserInvenSkillData userInvenSkillData)
    {
        _userInvenSkillData = userInvenSkillData;
        levelText.text = $"Lv.{userInvenSkillData.level}";

            skillIcon.gameObject.SetActive(true);
            skillIcon.sprite = Manager.Address.GetSprite(userInvenSkillData.itemID);
    }

    public void SetSlotEmpty()
    {
        levelText.text = string.Empty;
        skillIcon.gameObject.SetActive(false);
    }

    private void ShowPopupSkillDetailInfo()
    {
        if(_userInvenSkillData == null)
        {
            return;
        }

        var instancePopup = Manager.UI.ShowPopup<UIPopupSkillDetail>();
        instancePopup.SetSkillData(_userInvenSkillData);
    }
}
