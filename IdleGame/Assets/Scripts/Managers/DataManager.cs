using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager
{
    public GameUserProfile Profile { get; private set; }
    public InventoryData Inventory { get; private set; }
    public UserSkillData UserSkillData { get; private set; }
    public  Dictionary<string, UserInvenSkillData> SkillInvenDictionary = new();
    public UserFollowerData FollowerData { get; private set; }
    public Dictionary<string, UserInvenFollowerData> FollowerInvenDictionary = new();

    #region Create

    private void CreateUserFile()
    {
        Profile = new()
        {
            Uid = Utility.GenerateID(),
            Nickname = $"Guest-Test",
            Date_Login = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            Date_Logout = string.Empty,
            Date_Idle_ClickTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            Date_Bonus_ClickTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            Date_Bonus_Check = false,
            Gold = 0,
            Gems = 0,
            Stat_Level_AtkDamage = 1,
            Stat_Level_AtkSpeed = 1,
            Stat_Level_CritChance = 1,
            Stat_Level_CritDamage = 1,
            Stat_Level_Hp = 1,
            Stat_Level_HpRecovery = 1,
            Stage_Chapter = 1,
            Stage_Level = 1,
            Stage_WaveLoop = false,
            Quest_Complete = 0,
            Quest_Current_Progress = 0,
            Summon_Progress_Equipment = 0,
            Summon_Progress_Skills = 0,
            Summon_Progress_Follower = 0
        };

        SaveToUserProfile();
    }

    public void CreateUserEquipment()
    {
        var jsonData = Manager.Assets.GetTextUser("UserTableEquipment");
        Inventory = JsonUtility.FromJson<InventoryData>(jsonData);

        SaveToUserEquipment();
    }

    public void CreateUserSkill()
    {
        var jsonData = Manager.Assets.GetTextUser("UserTableSkill");
        UserSkillData = JsonUtility.FromJson<UserSkillData>(jsonData);

        SaveToUserSkill();
    }

    public void CreateUserFollower()
    {
        var jsonData = Manager.Assets.GetTextUser("UserTableFollower");
        FollowerData = JsonUtility.FromJson<UserFollowerData>(jsonData);

        SaveToUserFollower();
    }


    #endregion

    #region Load

    public IEnumerator Load()
    {
        LoadFromUserProfile();
        LoadFromUserEquipment();
        LoadFromUserSkill();
        LoadFromUserFollower();
        Debug.Log($"Load From {Application.persistentDataPath}");
        
        yield return null;
    }

    public void LoadFromUserProfile(string fileName = "game_user.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        if (!File.Exists(filePath)) { CreateUserFile(); return; }
        string jsonRaw = File.ReadAllText(filePath);
        Profile = JsonConvert.DeserializeObject<GameUserProfile>(jsonRaw);
    }

    public void LoadFromUserEquipment(string fileName = "game_equipment.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        if (!File.Exists(filePath)) { CreateUserEquipment(); return; }
        string jsonRaw = File.ReadAllText(filePath);
        Inventory = JsonConvert.DeserializeObject<InventoryData>(jsonRaw);
    }

    public void LoadFromUserSkill(string fileName = "game_skill.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        if (!File.Exists(filePath)) { CreateUserSkill(); return; }
        string jsonRaw = File.ReadAllText(filePath);
        UserSkillData = JsonConvert.DeserializeObject<UserSkillData>(jsonRaw);
        foreach(var  item in UserSkillData.UserInvenSkill)
        {
            SkillInvenDictionary.Add(item.itemID, item);
        }
    }
    
    public void LoadFromUserFollower(string fileName = "game_follower.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        if (!File.Exists(filePath)) { CreateUserFollower(); return; }
        string jsonRaw = File.ReadAllText(filePath);
        FollowerData = JsonConvert.DeserializeObject<UserFollowerData>(jsonRaw);
        foreach (var item in FollowerData.UserInvenFollower)
        {
            FollowerInvenDictionary.Add(item.itemID, item);
        }
    }

    #endregion

    #region Save

    public void Save()
    {
        SaveProfile();

        SaveToUserProfile();
        SaveToUserEquipment();
        SaveToUserSkill();
        SaveToUserFollower();
    }

    private void SaveProfile()
    {
        Profile.Date_Logout = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Profile.Date_Idle_ClickTime = Manager.Game.Player.IdleCheckTime.ToString("yyyy/MM/dd HH:mm:ss");
        Profile.Date_Bonus_ClickTime = Manager.Game.Player.BonusCheckTime.ToString("yyyy/MM/dd HH:mm:ss");
        Profile.Date_Bonus_Check = Manager.Game.Player.IsBonusCheck;
        Profile.Gold = Manager.Game.Player.Gold;
        Profile.Gems = Manager.Game.Player.Gems;
        Profile.Stat_Level_AtkDamage = Manager.Game.Player.AtkDamage.Level;
        Profile.Stat_Level_AtkSpeed = Manager.Game.Player.AtkSpeed.Level;
        Profile.Stat_Level_CritChance = Manager.Game.Player.CritChance.Level;
        Profile.Stat_Level_CritDamage = Manager.Game.Player.CritDamage.Level;
        Profile.Stat_Level_Hp = Manager.Game.Player.Hp.Level;
        Profile.Stat_Level_HpRecovery = Manager.Game.Player.HpRecovery.Level;
        Profile.Stage_Chapter = Manager.Stage.Chapter;
        Profile.Stage_Level = Manager.Stage.StageLevel;
        Profile.Stage_WaveLoop = Manager.Stage.WaveLoop;
        Profile.Quest_Complete = Manager.Quest.QuestNum;
        Profile.Summon_Progress_Equipment = Manager.Summon.GetSummonCounts("Equipment");
        Profile.Summon_Progress_Skills = Manager.Summon.GetSummonCounts("Skills");
        Profile.Summon_Progress_Follower = Manager.Summon.GetSummonCounts("Follower");
    }

    public void SaveToUserProfile(string fileName = "game_user.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        string json = JsonConvert.SerializeObject(Profile, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void SaveToUserEquipment(string fileName = "game_equipment.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        string json = JsonConvert.SerializeObject(Inventory, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void SaveToUserSkill(string fileName = "game_skill.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        string json = JsonConvert.SerializeObject(UserSkillData, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
    
    public void SaveToUserFollower(string fileName = "game_follower.dat")
    {
        string filePath = $"{Application.persistentDataPath}/{fileName}";
        string json = JsonConvert.SerializeObject(FollowerData, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    #endregion

    #region Firestore Save

    public void SetUserProfile(GameUserProfile profile)
    {
        Profile = profile;
        Profile.Date_Login = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    /// <summary>
    /// 유저 첫 생성 시, 기본 데이터 부여
    /// </summary>
    /// <param name="guestId"></param>
    /// <returns></returns>
    public GameUserProfile CreateUserProfile(string guestId)
    {
        Profile = new()
        {
            Uid = Utility.GenerateID(),
            Nickname = $"Guest-{guestId}",
            Date_Login = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            Date_Logout = string.Empty,
            Date_Idle_ClickTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            Date_Bonus_ClickTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            Date_Bonus_Check = false,
            Gold = 0,
            Gems = 0,
            Stat_Level_AtkDamage = 1,
            Stat_Level_AtkSpeed = 1,
            Stat_Level_CritChance = 1,
            Stat_Level_CritDamage = 1,
            Stat_Level_Hp = 1,
            Stat_Level_HpRecovery = 1,
            Stage_Chapter = 1,
            Stage_Level = 1,
            Stage_WaveLoop = false,
            Quest_Complete = 0,
            Quest_Current_Progress = 0
        };

        return Profile;
    }

    /// <summary>
    /// 파이어 스토어 저장
    /// </summary>
    public void SaveData()
    {
        Dictionary<string, object> updates = new()
        {
            { "Date_Login", Profile.Date_Login },
            { "Date_Logout", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
            { "Date_Idle_ClickTime", Manager.Game.Player.IdleCheckTime.ToString("yyyy/MM/dd HH:mm:ss") },
            { "Date_Bonus_ClickTime", Manager.Game.Player.BonusCheckTime.ToString("yyyy/MM/dd HH:mm:ss") },
            { "Date_Bonus_Check", Manager.Game.Player.IsBonusCheck },
            { "Gold", Manager.Game.Player.Gold },
            { "Gems", Manager.Game.Player.Gems },
            { "Stat_Level_AtkDamage", Manager.Game.Player.AtkDamage.Level },
            { "Stat_Level_AtkSpeed", Manager.Game.Player.AtkSpeed.Level },
            { "Stat_Level_CritChance", Manager.Game.Player.CritChance.Level },
            { "Stat_Level_CritDamage", Manager.Game.Player.CritDamage.Level },
            { "Stat_Level_Hp", Manager.Game.Player.Hp.Level },
            { "Stat_Level_HpRecovery", Manager.Game.Player.HpRecovery.Level },
            { "Stage_Chapter", Manager.Stage.Chapter },
            { "Stage_Level", Manager.Stage.StageLevel },
            { "Stage_WaveLoop", Manager.Stage.WaveLoop },
            { "Quest_Complete", Manager.Quest.QuestNum },
        };

        Manager.Session.UpdateUserData(updates);
    }

    #endregion
}
