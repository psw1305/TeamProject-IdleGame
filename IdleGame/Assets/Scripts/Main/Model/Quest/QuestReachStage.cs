using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReachStage : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.StageClear;
        questObjective = "Reach Staege";
        ValueUpRate = 2;
        objectiveValue = (questLevel / questCount) * ValueUpRate + 2;
        currentValue = Manager.Data.Profile.Stage_Level;
        isClear = currentValue > objectiveValue;
    }
}
