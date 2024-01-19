using Firebase.Firestore;

[FirestoreData]
public class GameUserProfile
{
    // Info
    [FirestoreProperty] public string Uid { get; set; }
    [FirestoreProperty] public string Nickname { get; set; }
    [FirestoreProperty] public long Gold { get; set; }
    [FirestoreProperty] public int Gems { get; set; }

    // Stats
    [FirestoreProperty] public int Stat_Level_Hp { get; set; }
    [FirestoreProperty] public int Stat_Level_HpRecovery { get; set; }
    [FirestoreProperty] public int Stat_Level_AtkDamage { get; set; }
    [FirestoreProperty] public int Stat_Level_AtkSpeed { get; set; }
    [FirestoreProperty] public int Stat_Level_CritChance { get; set; }
    [FirestoreProperty] public int Stat_Level_CritDamage { get; set; }

    // Stages
    [FirestoreProperty] public int Stage { get; set; }
    [FirestoreProperty] public int Stage_Level { get; set; }

    // Quest
    [FirestoreProperty] public int Quest_Complete { get; set; }
    [FirestoreProperty] public int Quest_Current_Progress { get; set; }
}
