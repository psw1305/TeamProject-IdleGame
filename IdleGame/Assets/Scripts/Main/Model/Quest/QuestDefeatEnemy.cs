using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDefeatEnemy : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.DefeatEnemy;
        questObjective = "Defeat Enemy";
        ValueUpRate = 1;
        objectiveValue = 5;
        currentValue = 0;
        isClear = currentValue > objectiveValue;
    }
}