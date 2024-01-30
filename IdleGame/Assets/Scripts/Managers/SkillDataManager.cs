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
        _skillDataBaseText = Manager.Resource.GetFileText("SkillDataTable");
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
    public string rarity;
    public float skillDamage;
    public float reinforceDamage;
    public float retentionEffect;
    public float reinforceEffect;
}