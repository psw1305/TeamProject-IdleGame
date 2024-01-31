using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopupSkillSlots : MonoBehaviour
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

    [SerializeField] private TextMeshProUGUI _lvTxt;
    [SerializeField] private GameObject _equippdText;

    [SerializeField] private Image reinforceProgress;
    [SerializeField] private TextMeshProUGUI reinforceText;

    [SerializeField] private Image itemSprite;

    [SerializeField] private GameObject lockCover;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private GameObject ReinforceIcon;


    private UserInvenSkillData _skillData;
    public UserInvenSkillData SkillData => _skillData;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetReinforceUI += SetReinforceData;
        SetReinforceUI += SetReinforceProgress;
        SetReinforceUI += SetReinforceIcon;
    }

    private void OnDestroy()
    {
        SetReinforceUI -= SetReinforceData;
        SetReinforceUI -= SetReinforceProgress;
        SetReinforceUI -= SetReinforceIcon;
    }

    #endregion

    #region Other Method

    //아이템 아이콘 세팅, 티어 세팅, 레벨 세팅,게이지 세팅, 언록 여부 
    public void InitSlotInfo(UserInvenSkillData skillData)
    {
        _skillData = skillData;
        _itemID = _skillData.itemID;
        _level = _skillData.level;
        _rarity = Manager.SkillData.SkillDataDictionary[skillData.itemID].rarity;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _skillData.hasCount;
    }

    public void InitSlotUI()
    {
        _lvTxt.text = $"Lv : {_level}";

        itemSprite.sprite = Manager.Resource.GetSprite(ItemID.ToString());

        SetLockState();
        gameObject.GetComponent<Button>().onClick.AddListener(ShowPopupSkillDetailInfo);
    }

    public void CheckEquipState()
    {
        if (_skillData.equipped == false)
        {
            _equippdText.SetActive(false);
        }
        else
        {
            _equippdText.SetActive(true);
        }
    }

    private void SetReinforceData()
    {
        _level = _skillData.level;
        _lvTxt.text = $"Lv. {_level}";
        _hasCount = _skillData.hasCount;
        if (_skillData.level < 15)
        {
            _needCount = _skillData.level + 1;
        }
        else
        {
            _needCount = 15;
        }
    }

    private void SetReinforceProgress()
    {
        reinforceProgress.fillAmount = (float)_hasCount / _needCount;
        reinforceText.text = $"{_hasCount}/{_needCount}";
    }

    private void SetReinforceIcon()
    {
        if (SkillData.hasCount >= _needCount)
        {
            ReinforceIcon.SetActive(true);
        }
        else
        {
            ReinforceIcon.SetActive(false);
        }
    }

    public void SetLockState()
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
