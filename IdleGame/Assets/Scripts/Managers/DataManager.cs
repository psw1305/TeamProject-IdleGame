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
            Stat_Level_AtkDamage = 1,
            Stat_Level_AtkSpeed = 1,
            Stat_Level_CritChance = 1,
            Stat_Level_CritDamage = 1,
            Stat_Level_Hp = 1,
            Stat_Level_HpRecovery = 1,
            Stage = 1,
            Stage_Level = 1
        };

        return Profile;
    }
}
