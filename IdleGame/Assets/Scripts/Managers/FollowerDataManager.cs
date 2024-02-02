using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FollowerDataManager
{
    private string _followerDataBaseText;
    private FollowerDataBase _follwerData;
    private Dictionary<string, FollowerData> _followerDataDictionary = new();
    public Dictionary<string, FollowerData> FollowerDataDictionary => _followerDataDictionary;

    public void ParseFollowerData()
    {
        _followerDataBaseText = Manager.Resource.GetFileText("DataTableFollower");
        _follwerData = JsonUtility.FromJson<FollowerDataBase>(_followerDataBaseText);
        foreach (var followerData in _follwerData.FollowerDataList)
        {
            _followerDataDictionary.Add(followerData.itemID, followerData);
        }
    }
    public void InitFollower()
    {
        ParseFollowerData();
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
        //EquipFollowerList = Manager.Data.FollowerData.UserInvenFollower.Where(followerData => followerData.equipped == true).ToList();
    }

    public UserInvenFollowerData SearchFollower(string itemID)
    {
        List<UserInvenFollowerData> pickFollower = UserFollowerData.UserInvenFollower.Where(followerData => followerData.itemID == itemID).ToList();
        return pickFollower[0];
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

[System.Serializable]
public class FollowerDataBase
{
    public List<FollowerData> FollowerDataList;
}

[System.Serializable]
public class FollowerData
{
    public string itemID;
    public string followerName;
    public string rarity;
    public float damageCorrection;
    public float atkSpeed;
    public float reinforceDamage;
    public float retentionEffect;
    public float reinforceEffect;
}