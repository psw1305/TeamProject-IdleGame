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

    #region slot info update

    private event Action<int> SetSkillUIEquipSlot;
    private event Action<string> SetSkillUIInvenSlot;
    private event Action SetAllSkillUIEquipSlot;

    //스킬 팝업 상단 : 장착 슬롯의 단일 업데이트 위한 메서드를 구독, 구독 해제
    public void AddSetSkillUIEquipSlot(Action<int> handler)
    {
        SetSkillUIEquipSlot += handler;
    }
    public void RemoveSetSkillUIEquipSlot(Action<int> handler)
    {
        SetSkillUIEquipSlot -= handler;
    }
    /// <summary>
    /// 스킬 팝업 상단 : 장착 슬롯의 단일 업데이트 위한 메서드입니다.
    /// </summary>
    /// <param name="id"></param>
    public void CallSetUISkillEquipSlot(int index)
    {
        SetSkillUIEquipSlot?.Invoke(index);
    }


    //스킬 팝업 상단 : 장착 슬롯의 일괄 업데이트 위한 메서드를 구독, 구독 해제
    public void AddSetAllSkillUIEquipSlot(Action handler)
    {
        SetAllSkillUIEquipSlot += handler;
    }
    public void RemoveSetAllSkillUIEquipSlot(Action handler)
    {
        SetAllSkillUIEquipSlot -= handler;
    }
    /// <summary>
    /// 스킬 팝업 상단 : 장착 슬롯의 일괄 업데이트 위한 메서드입니다.
    /// </summary>
    /// <param name="id"></param>
    public void CallSetAllSkillUIEquipSlot()
    {
        SetAllSkillUIEquipSlot.Invoke();
    }


    //스킬 팝업 하단 : 인벤토리 슬롯 상태를 업데이트하기 위한 메서드를 구독, 구독 해제
    public void AddSetSkillUIInvenSlot(Action<string> handler)
    {
        SetSkillUIInvenSlot += handler;
    }
    public void RemoveSetSkillUIInvenSlot(Action<string> handler)
    {
        SetSkillUIInvenSlot -= handler;
    }
    /// <summary>
    /// 스킬 팝업 하단 : 인벤토리 슬롯 상태를 업데이트하기 위한 메서드입니다.
    /// </summary>
    /// <param name="id"></param>
    public void CallSetUISkillInvenSlot(string id)
    {
        SetSkillUIInvenSlot.Invoke(id);
    }

    #endregion

    #region Equip Method

    /// <summary>
    /// userInvenSkillData 장착 시도 후 성공 시 슬롯의 인덱스, 실패 시 감시값을 반환합니다.
    /// <para> -100 : 해당 아이템을 보유하지 않아 착용 불가, -200 : 장착 가능한 슬롯이  없음</para>
    /// </summary>
    /// <param name="userInvenSkillData"></param>
    /// <returns></returns>
    public int EquipSkill(UserInvenSkillData userInvenSkillData)
    {
        //해당 아이템을 1개 이상 보유했었는지 확인
        if (userInvenSkillData.level == 1 && userInvenSkillData.hasCount == 0)
        {
            SystemAlertFloating.Instance.ShowMsgAlert(MsgAlertType.CanNotEquip);
            return -100;
        }

        //빈 슬롯을 찾는다면 해당 위치에 스킬을 착용함
        int index = Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == "Empty");
        if (index > -1)
        {
            Manager.Data.UserSkillData.UserEquipSkill[index].itemID = userInvenSkillData.itemID;
            Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ChangeEquipSkillData(index);
            userInvenSkillData.equipped = true;
            return index;
        }
        //빈 슬롯을 찾지 못한다면 -200을 반환함
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
        //해당아이템이 착용되어있는 인덱스를 찾음
        int index = Manager.Data.UserSkillData.UserEquipSkill.FindIndex(data => data.itemID == userInvenSkillData.itemID);
        //초기화 작업
        Manager.Data.UserSkillData.UserEquipSkill[index].itemID = "Empty";
        Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ChangeEquipSkillData(index);
        userInvenSkillData.equipped = false;
        return index;
    }

    #endregion

    #region Reinforce Method

    //강화 로직
    private void ReinforceSkill(UserInvenSkillData userInvenSkillData)
    {
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

    /// <summary>
    /// userInvenSkillData로 전달받은 스킬을 강화합니다.
    /// </summary>
    /// <param name="userInvenSkillData"></param>
    public void ReinforceSelectSkill(UserInvenSkillData userInvenSkillData)
    {
        if (userInvenSkillData.hasCount < Mathf.Min(userInvenSkillData.level + 1, 15))
        {
            SystemAlertFloating.Instance.ShowMsgAlert(MsgAlertType.CanNotReinforce);
            return;
        }
        else if (userInvenSkillData.level >= 100 & (userInvenSkillData.itemID == Manager.Data.SkillInvenList.Last().itemID))
        {
            SystemAlertFloating.Instance.ShowMsgAlert(MsgAlertType.ItemLimitLevel);
            return;
        }

        ReinforceSkill(userInvenSkillData);
    }

    /// <summary>
    /// 스킬을 일괄 강화 합니다.
    /// </summary>
    public void ReinforceAllSkill()
    {
        var list = Manager.Data.SkillInvenList.Where(item => item.hasCount >= Mathf.Min(item.level + 1, 15));

        if (list.Count() == 0 || (list.First().itemID == Manager.Data.SkillInvenList.Last().itemID & list.First().level >= 100))
        {
            Debug.Log(list.Count());
            SystemAlertFloating.Instance.ShowMsgAlert(MsgAlertType.CanNotAllReinforce);
            return;
        }

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