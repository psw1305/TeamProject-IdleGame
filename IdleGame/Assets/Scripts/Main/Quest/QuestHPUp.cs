using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHPUp : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.HPUp;
        questObjective = "HP Upgrade";
        ValueUpRate = 2;
        objectiveValue = (questLevel / questCount) < 1 ? 10 : (questLevel / questCount) * ValueUpRate * 10; 
        currentValue = Manager.Data.Profile.Stat_Level_Hp;
        isClear = currentValue > objectiveValue;
    }
}