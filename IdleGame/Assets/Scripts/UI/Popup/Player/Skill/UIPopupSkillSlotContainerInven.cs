using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkillSlotContainerInven : MonoBehaviour
{
    #region Fields

    public Dictionary<string, UIPopupSkillSlotsInven> SkillSlots = new Dictionary<string, UIPopupSkillSlotsInven>();
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

        Manager.SkillData.AddSetSkillUIInvenSlot(SetUISlotEquipState);
        Manager.SkillData.AddSetSkillUIInvenSlot(SetUISlotReinforceState);
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
            UIPopupSkillSlotsInven slot = Manager.Address.InstantiatePrefab("Slot_Skill", gameObject.transform).GetComponent<UIPopupSkillSlotsInven>();
            SkillSlots.Add(itemData.itemID, slot);
            slot.InitSlotInfo(itemData);
            slot.InitSlotUI();
            slot.SetUIEquipState();
            slot.SetReinforceUI();
        }

        ResetOnScrollTop();
    }

    //슬롯에 장착 여부를 춫력하기 위한 메서드
    public void SetUISlotEquipState()
    {
        foreach (var slot in SkillSlots)
        {
            slot.Value.SetUIEquipState();
        }
    }
    public void SetUISlotEquipState(string id)
    {
        SkillSlots[id].SetUIEquipState();
    }

    public void SetUISlotReinforceState()
    {
        foreach (var slot in SkillSlots)
        {
            slot.Value.SetReinforceUI();
        }
    }
    public void SetUISlotReinforceState(string id)
    {
        SkillSlots[id].SetReinforceData();
        SkillSlots[id].SetUIReinforceIcon();
    }

    private void OnDestroy()
    {
        if (Manager.SkillData != null)
        {
            Manager.SkillData.RemoveSetSkillUIInvenSlot(SetUISlotEquipState);
            Manager.SkillData.RemoveSetSkillUIInvenSlot(SetUISlotReinforceState);
        }
    }

    #endregion
}
