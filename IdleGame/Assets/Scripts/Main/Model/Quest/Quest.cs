using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct testQuestData
{
    public QuestType questType;
    public string questObjective;
    public int objectiveValue;
    public int currentValue;
    public bool isClear;
}

[System.Serializable]
public abstract class Quest
{
    public testQuestData data;
    public QuestDataBase QuestDataBase;

    public int QuestNum;
    public int QuestIndex;
    public List<QuestData> QuestDB;
    public List<testQuestData> testQuestDatas;

    public abstract void Init();

    public virtual void NextQuest()
    {
    }

    public void EarnQuestReward()
    {
        Manager.Game.Player.RewardGem(500);
    }
}