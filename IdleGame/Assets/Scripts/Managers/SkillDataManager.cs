using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager
{
    private string _skillDataBaseText;
    private SkillDataBase _skillData;
    private Dictionary<string, SkillData> _skillDataDictionary = new();

    public Dictionary<string, SkillData> SkillDataDictionary => _skillDataDictionary;

    public void ParseSkillData()
    {
        _skillDataBaseText = Manager.Address.GetText("ItemTableSkill");
        _skillData = JsonUtility.FromJson<SkillDataBase>(_skillDataBaseText);
        foreach (var skillData in _skillData.SkillDataList)
        {
            _skillDataDictionary.Add(skillData.itemID, skillData);
        }
    }

    public void InitSkill()
    {
        ParseSkillData();
    }

    private event Action<int?> SetSkillUIEquipSlot;
    private event Action<string> SetSkillUIInvenSlot;

    public void AddSetSkillUIEquipSlot(Action<int?> handler)
    {
        SetSkillUIEquipSlot += handler;
    }
    public void RemoveSetSkillUIEquipSlot(Action<int?> handler)
    {
        SetSkillUIEquipSlot -= handler;
    }
    public void AddSetSkillUIInvenSlot(Action<string> handler)
    {
        SetSkillUIInvenSlot += handler;
    }
    public void RemoveSetSkillUIInvenSlot(Action<string> handler)
    {
        SetSkillUIInvenSlot -= handler;
    }

    public void CallSetUISkillEquipSlot(int? index)
    {
        SetSkillUIEquipSlot?.Invoke(index);
    }
    public void CallSetUISkillInvenSlot(string id)
    {
        SetSkillUIInvenSlot.Invoke(id);
    }

    public bool CheckEquipSkill(UserInvenSkillData userInvenSkillData)
    {
        return Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == "Empty") > -1 ? true : false;
    }

    public int? EquipSkill(UserInvenSkillData userInvenSkillData)
    {
        if (userInvenSkillData.level == 1 && userInvenSkillData.hasCount == 0)
        {
            return null;
        }
        int index = Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == "Empty");
        if (index > -1)
        {
            Manager.Data.UserSkillData.UserEquipSkill[index].itemID = userInvenSkillData.itemID;
            userInvenSkillData.equipped = true;
            return index;
        }
        return null;
    }

    public int UnEquipSkill(UserInvenSkillData userInvenSkillData)
    {
        int index = Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == userInvenSkillData.itemID);
        Manager.Data.UserSkillData.UserEquipSkill[index].itemID = "Empty";
        userInvenSkillData.equipped = false;
        return index;
    }

    public void ReinforceSkill(UserInvenSkillData userInvenSkillData)
    {
        if (userInvenSkillData.hasCount < Mathf.Min(userInvenSkillData.level + 1, 15))
        {
            return;
        }
        while (userInvenSkillData.hasCount >= Mathf.Min(userInvenSkillData.level + 1, 15))
        {
            userInvenSkillData.hasCount -= Mathf.Min(userInvenSkillData.level + 1, 15);
            userInvenSkillData.level += 1;
        }
    }

    public UserInvenSkillData SearchSkill(string id)
    {
        return Manager.Data.SkillInvenDictionary[id];
    }

    public void ReinforceAllSkill()
    {
        foreach (var item in Manager.Data.UserSkillData.UserInvenSkill)
        {
            ReinforceSkill(item);
        }
    }

}

[System.Serializable]
public class UserSkillData
{
    public List<UserEquipSkillData> UserEquipSkill;
    public List<UserInvenSkillData> UserInvenSkill;
}

[System.Serializable]
public class UserEquipSkillData
{
    public string itemID;
}

[System.Serializable]
public class UserInvenSkillData
{
    public string itemID;
    public int level;
    public int hasCount;
    public bool equipped;
}


[System.Serializable]
public class SkillDataBase
{
    public List<SkillData> SkillDataList;
}

[System.Serializable]
public class SkillData
{
    public string itemID;
    public string skillName;
    public string description;
    public string rarity;
    public float skillDamage;
    public float reinforceDamage;
    public float retentionEffect;
    public float reinforceEffect;
}