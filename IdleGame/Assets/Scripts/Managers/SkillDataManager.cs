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
    public UserSkill UserSkill { get; private set; }
}


[System.Serializable]
public class UserSkill
{
    public List<UserSkillData> UserSkillData;
}

[System.Serializable]
public class UserSkillData
{
    public string itemID;
    public int level;
    public int hasCount;
    public bool equipped;

    public UserSkillData(string ItemID, int Level, int HasCount)
    {
        itemID = ItemID;
        level = Level;
        hasCount = HasCount;
    }
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