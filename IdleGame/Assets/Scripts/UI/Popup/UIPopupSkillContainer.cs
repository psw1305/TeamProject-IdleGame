using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkillContainer : MonoBehaviour
{
    #region Fields

    public List<GameObject> SkillSlots = new List<GameObject>();
    public GameObject itemInfoUI;
    private ScrollRect scrollRect;

    [SerializeField] private UIPopupSkill MainPopupUI;

    #endregion


    #region Unity Flow

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    private void Start()
    {
        MainPopupUI = Manager.UI.CurrentPopup as UIPopupSkill;
        InitSlot();
        MainPopupUI.RefreshReinforecEvent += SetSkillSlotUI;
        MainPopupUI.RefreshReinforecEvent += SetSkillSlotReinforceUI;
    }
    #endregion

    #region Initial Method

    private void ResetOnScrollTop()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

    //JSON에서 파싱한 아이템 개수 만큼 슬롯을 생성합니다.
    public void InitSlot()
    {
        SkillSlots.Clear();

        foreach (var itemData in Manager.Data.UserSkillData.UserInvenSkill)
        {
            GameObject slot = Manager.Resource.InstantiatePrefab("Img_SkillSlot", gameObject.transform);
            SkillSlots.Add(slot);
            slot.GetComponent<UIPopupSkillSlots>().InitSlotInfo(itemData);
            slot.GetComponent<UIPopupSkillSlots>().InitSlotUI();
            slot.GetComponent<UIPopupSkillSlots>().CheckEquipState();
            slot.GetComponent<UIPopupSkillSlots>().SetReinforceUI();
        }

        ResetOnScrollTop();
    }

    //슬롯에 장착 여부를 춫력하기 위한 메서드
    public void SetSkillSlotUI()
    {
        foreach (var slot in SkillSlots)
        {
            slot.GetComponent<UIPopupSkillSlots>().CheckEquipState();
        }
    }
    public void SetSkillSlotReinforceUI()
    {
        foreach (var slot in SkillSlots)
        {
            slot.GetComponent<UIPopupSkillSlots>().SetReinforceUI();
        }
    }

    #endregion
}
