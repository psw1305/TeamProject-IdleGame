using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FollowerDataManager
{
    private FollowerContainerBlueprint _follwerDataContainer;

    private Dictionary<string, FollowerBlueprint> _followerDataDictionary = new();
    public Dictionary<string, FollowerBlueprint> FollowerDataDictionary => _followerDataDictionary;

    public void ParseFollowerData()
    {
        _follwerDataContainer = Manager.Address.GetBlueprint("FollowerDataContainer") as FollowerContainerBlueprint;
        //_follwerData = JsonUtility.FromJson<FollowerDataBase>(_followerDataBaseText);
        foreach (var followerData in _follwerDataContainer.followerDatas)
        {
            _followerDataDictionary.Add(followerData.ItemID, followerData);
        }
    }
    public void InitFollower()
    {
        ParseFollowerData();
    }

    public event Action<int?> SetFollowerUIEquipSlot;
    public event Action<string> SetFollowerUIInvenSlot;

    public void AddSetFollowerUIEquipSlot(Action<int?> handler)
    {
        SetFollowerUIEquipSlot += handler;
    }

    public void RemoveSetFollowerUIEquipSlot(Action<int?> handler)
    {
        SetFollowerUIEquipSlot -= handler;
    }

    public void AddSetFollowerUIInvenSlot(Action<string> handler)
    {
        SetFollowerUIInvenSlot += handler;
    }

    public void RemoveSetFollowerUIInvenSlot(Action<string> handler)
    {
        SetFollowerUIInvenSlot -= handler;
    }

    public void CallSetUIFollowerEquipSlot(int? index)
    {
        SetFollowerUIEquipSlot?.Invoke(index);
    }

    public void CallSetUIFollowerInvenSlot(string id)
    {
        SetFollowerUIInvenSlot.Invoke(id);
    }

    public bool CheckEquipFollower(UserInvenFollowerData userInvenFollowerData)
    {
        return Manager.Data.FollowerData.UserEquipFollower.FindIndex(data => data.itemID == "Empty") > -1 ? true : false;
    }

    public int? EquipFollower(UserInvenFollowerData userInvenFollowerData)
    {
        if (userInvenFollowerData.level == 1 && userInvenFollowerData.hasCount == 0)
        {
            return null;
        }
        int index = Manager.Data.FollowerData.UserEquipFollower.FindIndex(data => data.itemID == "Empty");
        if (index > -1)
        {
            Manager.Data.FollowerData.UserEquipFollower[index].itemID = userInvenFollowerData.itemID;
            userInvenFollowerData.equipped = true;
            return index;
        }
        return null;
    }

    public int UnEquipFollower(UserInvenFollowerData userInvenFollowerData)
    {
        int index = Manager.Data.FollowerData.UserEquipFollower.FindIndex(data => data.itemID == userInvenFollowerData.itemID);
        Manager.Data.FollowerData.UserEquipFollower[index].itemID = "Empty";
        userInvenFollowerData.equipped = false;
        return index;
    }

    public void ReinforceFollower(UserInvenFollowerData userInvenFollowerData)
    {
        if (userInvenFollowerData.hasCount < Mathf.Min(userInvenFollowerData.level + 1, 15))
        {
            return;
        }
        while (userInvenFollowerData.hasCount >= Mathf.Min(userInvenFollowerData.level + 1, 15))
        {
            userInvenFollowerData.hasCount -= Mathf.Min(userInvenFollowerData.level + 1, 15);
            userInvenFollowerData.level += 1;
        }
        CallSetUIFollowerInvenSlot(userInvenFollowerData.itemID);
        Manager.Notificate.SetReinforceFollowerNoti();
        Manager.Notificate.SetPlayerStateNoti();
    }
    public void ReinforceAllFollower()
    {
        foreach (var item in Manager.Data.FollowerData.UserInvenFollower)
        {
            ReinforceFollower(item);
        }
    }

    #region FollowerData Fields & properties

    public UserFollowerData UserFollowerData { get; private set; }
    public List<UserEquipFollowerData> EquipFollowerList { get; private set; }
    public List<UserInvenFollowerData> InvenFollowerList { get; private set; }

    #endregion

    public void Initialize()
    {
        UserFollowerData = Manager.Data.FollowerData;
        InvenFollowerList = Manager.Data.FollowerData.UserInvenFollower.Where(followerData => followerData.itemID[0] == 'F').ToList();
    }

    public UserInvenFollowerData SearchFollower(string itemID)
    {
        return Manager.Data.FollowerInvenDictionary[itemID];
    }

}

[System.Serializable]
public class UserFollowerData
{
    public List<UserEquipFollowerData> UserEquipFollower;
    public List<UserInvenFollowerData> UserInvenFollower;
}

[System.Serializable]
public class UserEquipFollowerData
{
    public string itemID;
}

[System.Serializable]
public class UserInvenFollowerData
{
    public string itemID;
    public int level;
    public int hasCount;
    public bool equipped;
}