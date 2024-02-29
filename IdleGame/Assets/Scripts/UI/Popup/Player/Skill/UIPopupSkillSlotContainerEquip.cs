using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupSkillSlotContainerEquip : MonoBehaviour
{
    private UIPopupSkillSlotsEquip[] _slots = new UIPopupSkillSlotsEquip[5];

    [SerializeField] private GameObject _DimCover;


    private void Start()
    {
        _slots = gameObject.GetComponentsInChildren<UIPopupSkillSlotsEquip>();

        InitChildSlot();
        Manager.SkillData.AddSetAllSkillUIEquipSlot(InitChildSlot);
        Manager.SkillData.AddSetSkillUIEquipSlot(SetChildSlot);
    }

    private void InitChildSlot()
    {
        for (int i = 0; i < Manager.Data.UserSkillData.UserEquipSkill.Count; i++)
        {
            if (Manager.Data.UserSkillData.UserEquipSkill[i].itemID == "Empty")
                _slots[i].SetSlotEmpty();
            else
                _slots[i].SetSlotUI(Manager.Data.SkillInvenDictionary[Manager.Data.UserSkillData.UserEquipSkill[i].itemID]);
        }
    }

    /// <summary>
    /// 특정 인덱스의 스킬 장착 슬롯 정보를 세팅합니다.
    /// </summary>
    /// <param name="index"></param>
    private void SetChildSlot(int index)
    {
        if (index == -100)
        {
            return;
        }

        if (index == -200)
        {
            ToggleSlotReplaceMode();
            return;
        }

        if (Manager.Data.UserSkillData.UserEquipSkill[index].itemID == "Empty")
        {
            _slots[index].SetSlotEmpty();
        }
        else
        {
            _slots[index].SetSlotUI(Manager.Data.SkillInvenDictionary[Manager.Data.UserSkillData.UserEquipSkill[index].itemID]);
        }
    }

    public void ToggleSlotReplaceMode()
    {
        foreach (var slot in _slots)
        {
            slot.ReplaceMode = !slot.ReplaceMode;
        }
        _DimCover.gameObject.SetActive(!_DimCover.gameObject.activeSelf);
    }


    private void OnDestroy()
    {
        if (Manager.SkillData != null)
        {
            Manager.SkillData.RemoveSetAllSkillUIEquipSlot(InitChildSlot);
            Manager.SkillData.RemoveSetSkillUIEquipSlot(SetChildSlot);
        }
    }
}
