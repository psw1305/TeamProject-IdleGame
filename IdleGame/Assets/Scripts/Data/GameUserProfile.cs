using System.Collections.Generic;

[System.Serializable]
public class GameUserProfile
{
    // Info
    public string username;
    public string uid;
    public bool firstPurchaseCompleted;
    public bool removeAds;
    public string offlineTime;
    public string createdDate;
    public string lastTimePlayed;
    public long gold = 0;
    public int gems = 0;
    public string weaponId;
    public string armorId;

    // Stats
    public int hpStatLevel = 1;
    public long hpStatValue = 1000;
    public long hpUpgradeCost = 10;

    public int hpRecoveryStatLevel = 1;
    public long hpRecoveryStatValue = 30;
    public long hpRecoveryUpgradeCost = 10;

    public int atkDamageStatLevel = 1;
    public long atkDamageStatValue = 10;
    public long atkDamageUpgradeCost = 10;

    public int atkSpeedStatLevel = 1;
    public long atkSpeedStatValue = 500;
    public long atkSpeedUpgradeCost = 10;

    public int critChanceStatLvl = 1;
    public long critChanceStatValue = 500;
    public long critChanceUpgradeCost = 10;

    public int critDamageStatLevel = 1;
    public long critDamageStatValue = 1000;
    public long critDamageUpgradeCost = 10;

    // Stages
    public int stage = 1;
    public int stageLevel = 1;

    // Inventory

    // Quest
    public int completedQuests = 1;
    public string currentQuestType;
    public int currentQuestGoal;
    public int currentQuestProgress;
}
