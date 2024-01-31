using System.Collections.Generic;
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
}

[System.Serializable]
public class UserFollowerData
{
    public List<UserEquipFollowerData> UserEquipSkill;
    public List<UserInvenFollowerData> UserInvenFollowerData;
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
    public float reinforceDamage;
    public float retentionEffect;
    public float reinforceEffect;
}