using System;
using System.Collections.Generic;

public class DataManager
{
    public GameUserProfile Profile { get; private set; }

    public void SetUserProfile(GameUserProfile profile)
    {
        Profile = profile;
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
            Stat_Level_AtkDamage = 1,
            Stat_Level_AtkSpeed = 1,
            Stat_Level_CritChance = 1,
            Stat_Level_CritDamage = 1,
            Stat_Level_Hp = 1,
            Stat_Level_HpRecovery = 1,
            Stage_Difficulty = 1,
            Stage_Chapter = 1,
            Stage_Level = 1,
            Stage_WaveLoop = false,
            Quest_Complete = 0,
            Quest_Current_Progress = 0
        };

        return Profile;
    }

    #region Save

    /// <summary>
    /// TODO => 최적의 시기에 저장 2-3번에 처리
    /// </summary>
    public void SaveData()
    {
        Dictionary<string, object> updates = new()
        {
            { "Date_Logout", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
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
