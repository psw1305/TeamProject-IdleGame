using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager
{
    private SkillContainerBlueprint _skillDataContainer;

    private Dictionary<string, SkillBlueprint> _skillDataDictionary = new();
    public Dictionary<string, SkillBlueprint> SkillDataDictionary => _skillDataDictionary;

    public void InitSkill()
    {
        ParseSkillData();
    }

    public void ParseSkillData()
    {
        _skillDataContainer = Manager.Asset.GetBlueprint("SkillDataContainer") as SkillContainerBlueprint;
        //_skillData = JsonUtility.FromJson<SkillDataBase>(_skillDataContainer);
        foreach (var skillData in _skillDataContainer.skillDatas)
        {
            _skillDataDictionary.Add(skillData.ItemID, skillData);
        }
    }

    #region popup event actions

    private event Action<int?> SetSkillUIEquipSlot;
    private event Action<string> SetSkillUIInvenSlot;
    private event Action SetAllSkillUIEquipSlot;

    public void AddSetSkillUIEquipSlot(Action<int?> handler)
    {
        SetSkillUIEquipSlot += handler;
    }
    public void RemoveSetSkillUIEquipSlot(Action<int?> handler)
    {
        SetSkillUIEquipSlot -= handler;
    }
    public void CallSetUISkillEquipSlot(int? index)
    {
        SetSkillUIEquipSlot?.Invoke(index);
    }

    public void AddSetAllSkillUIEquipSlot(Action handler)
    {
        SetAllSkillUIEquipSlot += handler;
    }
    public void RemoveSetAllSkillUIEquipSlot(Action handler)
    {
        SetAllSkillUIEquipSlot -= handler;
    }
    public void CallSetAllSkillUIEquipSlot()
    {
        SetAllSkillUIEquipSlot.Invoke();
    }

    public void AddSetSkillUIInvenSlot(Action<string> handler)
    {
        SetSkillUIInvenSlot += handler;
    }
    public void RemoveSetSkillUIInvenSlot(Action<string> handler)
    {
        SetSkillUIInvenSlot -= handler;
    }
    public void CallSetUISkillInvenSlot(string id)
    {
        SetSkillUIInvenSlot.Invoke(id);
    }

    #endregion

    #region Equip, Reinforce Method

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
        CallSetUISkillInvenSlot(userInvenSkillData.itemID);
        CallSetAllSkillUIEquipSlot();
        Manager.Notificate.SetReinforceSkillNoti();
        Manager.Notificate.SetPlayerStateNoti();

        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    public void ReinforceAllSkill()
    {
        foreach (var item in Manager.Data.UserSkillData.UserInvenSkill)
        {
            ReinforceSkill(item);
        }
        CallSetAllSkillUIEquipSlot();

        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    #endregion

    public UserInvenSkillData SearchSkill(string id)
    {
        return Manager.Data.SkillInvenDictionary[id];
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