using System.Collections.Generic;
using UnityEngine;

public class UIUseSkillContainer : MonoBehaviour
{
    private List<UIUseSkillSlots> slots = new();
    private void Start()
    {
        for (int i = 0; i > gameObject.GetComponentsInChildren<UIUseSkillSlots>().Length; i++)
        {
            slots.Add(gameObject.GetComponentsInChildren<UIUseSkillSlots>()[i]);

            Debug.LogWarning(slots[i]);
            if (Manager.Data.UserSkillData.UserEquipSkill[i].itemID == "Empty")
            {
                slots[i].SkillIcon.gameObject.SetActive(false);
            }
            else
            {
                slots[i].SkillIcon.sprite = Manager.Resource.GetSprite(Manager.Data.UserSkillData.UserEquipSkill[i].itemID);
                slots[i].SkillIcon.gameObject.SetActive(true);
            }
        }
    }
}
