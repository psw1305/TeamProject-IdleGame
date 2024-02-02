using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupSkillSlotsInven : MonoBehaviour
{

    #region Value Fields

    private string _itemID;
    private int _level;
    private int _hasCount;
    private int _needCount;
    private string _rarity;

    #endregion

    #region Properties
    public string ItemID => _itemID;

    #endregion

    #region Object Fields

    //타 클래스에서 여러 메서드를 실행할때 콜백으로 하거나 여러번 GetCompornent 처리하는게 마음에 들지 않아 일단 Action으로 묶어두었음 
    public Action SetReinforceUI;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject equippdText;

    [SerializeField] private Image reinforceProgressSprite;
    [SerializeField] private TextMeshProUGUI reinforceProgressText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private GameObject reinforceIcon;


    private UserInvenSkillData _skillData;
    public UserInvenSkillData SkillData => _skillData;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetReinforceUI += SetReinforceData;
        SetReinforceUI += SetUIReinforceIcon;
    }

    private void OnDestroy()
    {
        SetReinforceUI -= SetReinforceData;
        SetReinforceUI -= SetUIReinforceIcon;
    }

    #endregion

    #region Other Method

    //아이템 아이콘 세팅, 티어 세팅, 레벨 세팅,게이지 세팅, 언록 여부 
    public void InitSlotInfo(UserInvenSkillData skillData)
    {
        _skillData = skillData;
        _itemID = _skillData.itemID;
        _level = _skillData.level;
        _hasCount = _skillData.hasCount;
        _rarity = Manager.SkillData.SkillDataDictionary[skillData.itemID].rarity;
    }

    public void InitSlotUI()
    {
        itemSprite.sprite = Manager.Resource.GetSprite(ItemID.ToString());
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupSkillDetailInfo);

        SetUILockState();
    }

    public void SetReinforceData()
    {
        levelText.text = $"Lv. {_skillData.level}";
        _needCount = _skillData.level < 15 ? _skillData.level + 1 : 15;
        reinforceProgressText.text = $"{_skillData.hasCount} / {_needCount}";
        reinforceProgressSprite.fillAmount = (float)_skillData.hasCount / _needCount;
    }

    public void SetUIEquipState()
    {
        if (_skillData.equipped == false)
        {
            equippdText.SetActive(false);
        }
        else
        {
            equippdText.SetActive(true);
        }
    }

    public void SetUIReinforceIcon()
    {
        if (SkillData.hasCount >= _needCount)
        {
            reinforceIcon.SetActive(true);
        }
        else
        {
            reinforceIcon.SetActive(false);
        }
    }

    public void SetUILockState()
    {
        if (_level > 1 || _hasCount > 0)
        {
            lockCover.SetActive(false);
            lockIcon.SetActive(false);
            return;
        }

        lockCover.SetActive(true);
        lockIcon.SetActive(true);
    }

    private void ShowPopupSkillDetailInfo()
    {
        var instancePopup =  Manager.UI.ShowPopup<UIPopupSkillDetail>();
        instancePopup.SetSkillData(_skillData);
    }

    #endregion
}
