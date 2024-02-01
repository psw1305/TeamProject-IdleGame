using System.Collections.Generic;
using UnityEngine;

public class UIPopupSkillSlotContainerEquip : MonoBehaviour
{
    private List<UIPopupSkillSlotsEquip> slots = new List<UIPopupSkillSlotsEquip>();

    private void Start()
    {
        foreach (var slot in gameObject.GetComponentsInChildren<UIPopupSkillSlotsEquip>())
        {
            slots.Add(slot);
        }
        InitChildSlot();
        Manager.SkillData.SetUISkillEquip += InitChildSlot;
    }

    private void InitChildSlot()
    {
        for (int i = 0; i <Manager.Data.UserSkillData.UserEquipSkill.Count; i++)
        {
            if (Manager.Data.UserSkillData.UserEquipSkill[i].itemID == "Empty")
            {
                continue;
            }
            slots[i].SetSlotUI(Manager.Data.SkillInvenDictionary[Manager.Data.UserSkillData.UserEquipSkill[i].itemID]);
        }
    }

    private void SetChildSlot(int index)
    {

    }

    private void OnDestroy()
    {
          if (Manager.SkillData != null)
        {
            Manager.SkillData.SetUISkillEquip -= InitChildSlot;
        }
    }
}
