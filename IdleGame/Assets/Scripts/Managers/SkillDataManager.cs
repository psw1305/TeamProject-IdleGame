using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillDataManager
{
    private SkillContainerBlueprint _skillDataContainer;

    private Dictionary<string, SkillBlueprint> _skillDataDictionary = new();
    public Dictionary<string, SkillBlueprint> SkillDataDictionary => _skillDataDictionary;

    public UserInvenSkillData ReplaceSkill;

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

    private event Action<int> SetSkillUIEquipSlot;
    private event Action<string> SetSkillUIInvenSlot;
    private event Action SetAllSkillUIEquipSlot;

    public void AddSetSkillUIEquipSlot(Action<int> handler)
    {
        SetSkillUIEquipSlot += handler;
    }
    public void RemoveSetSkillUIEquipSlot(Action<int> handler)
    {
        SetSkillUIEquipSlot -= handler;
    }
    public void CallSetUISkillEquipSlot(int index)
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

    /// <summary>
    /// userInvenSkillData 장착 시도 후 성공 시 슬롯의 인덱스, 실패 시 감시값을 반환합니다.
    /// <para> -100 : 해당 아이템을 보유하지 않아 착용 불가, -200 : 장착 가능한 슬롯이  없음</para>
    /// </summary>
    /// <param name="userInvenSkillData"></param>
    /// <returns></returns>
    public int EquipSkill(UserInvenSkillData userInvenSkillData)
    {
        if (userInvenSkillData.level == 1 && userInvenSkillData.hasCount == 0)
        {
            return -100;
        }

        int index = Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == "Empty");
        if (index > -1)
        {
            Manager.Data.UserSkillData.UserEquipSkill[index].itemID = userInvenSkillData.itemID;
            Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ChangeEquipSkillData(index);
            userInvenSkillData.equipped = true;
            return index;
        }
        ReplaceSkill = userInvenSkillData;
        return -200;
    }


    /// <summary>
    /// userInvenSkillData의 장착을 해제합니다.
    /// </summary>
    /// <param name="userInvenSkillData"></param>
    /// <returns></returns>
    public int UnEquipSkill(UserInvenSkillData userInvenSkillData)
    {
        int index = Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == userInvenSkillData.itemID);
        Manager.Data.UserSkillData.UserEquipSkill[index].itemID = "Empty";
        Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ChangeEquipSkillData(index);
        userInvenSkillData.equipped = false;
        return index;
    }

    public void ReinforceSkill(UserInvenSkillData userInvenSkillData)
    {
        //조건 미충족 시 리턴
        if (userInvenSkillData.hasCount < Mathf.Min(userInvenSkillData.level + 1, 15))
        {
            return;
        }

        while (userInvenSkillData.hasCount >= Mathf.Min(userInvenSkillData.level + 1, 15))
        {
            if (userInvenSkillData.level < 100)
            {
                userInvenSkillData.hasCount -= Mathf.Min(userInvenSkillData.level + 1, 15);
                userInvenSkillData.level += 1;
            }
            //최고 레벨
            else
            {
                int index = Manager.Data.SkillInvenList.FindIndex(item => item.itemID == userInvenSkillData.itemID);
                if (Manager.Data.SkillInvenList.Count - 1 > index)
                {
                    userInvenSkillData.hasCount -= Mathf.Min(userInvenSkillData.level + 1, 15);
                    Manager.Data.SkillInvenList[index + 1].hasCount += 1;
                }
                else if (Manager.Data.SkillInvenList.Last().level < 100)
                {
                    userInvenSkillData.hasCount -= Mathf.Min(userInvenSkillData.level + 1, 15);
                    Manager.Data.SkillInvenList[index + 1].hasCount += 1;
                }
                else
                {
                    break;
                }
            }
        }

        CallSetUISkillInvenSlot(userInvenSkillData.itemID);
        CallSetAllSkillUIEquipSlot();
    }

    public void ReinforceAllSkill()
    {
        foreach (var item in Manager.Data.UserSkillData.UserInvenSkill)
        {
            ReinforceSkill(item);
        }
        CallSetAllSkillUIEquipSlot();
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