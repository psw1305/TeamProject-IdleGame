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
        Manager.SkillData.SetSkillUIEquipSlot += SetChildSlot;
    }

    private void InitChildSlot()
    {
        for (int i = 0; i < Manager.Data.UserSkillData.UserEquipSkill.Count; i++)
        {
            if (Manager.Data.UserSkillData.UserEquipSkill[i].itemID == "Empty")
                slots[i].SetSlotEmpty();
            else
                slots[i].SetSlotUI(Manager.Data.SkillInvenDictionary[Manager.Data.UserSkillData.UserEquipSkill[i].itemID]);
        }
    }
    /// <summary>
    /// 특정 인덱스의 스킬 장착 슬롯 정보를 세팅합니다.
    /// </summary>
    /// <param name="index"></param>
    private void SetChildSlot(int? index)
    {
        if (index == null)
            return;
        if (Manager.Data.UserSkillData.UserEquipSkill[index.Value].itemID == "Empty")
            slots[index.Value].SetSlotEmpty();
        else
            slots[index.Value].SetSlotUI(Manager.Data.SkillInvenDictionary[Manager.Data.UserSkillData.UserEquipSkill[index.Value].itemID]);
        Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ChangeEquipSkillData(index.Value);
    }

    private void OnDestroy()
    {
        if (Manager.SkillData != null)
        {
            Manager.SkillData.SetSkillUIEquipSlot -= SetChildSlot;
        }
    }
}
