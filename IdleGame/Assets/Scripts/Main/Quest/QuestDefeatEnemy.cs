public class QuestDefeatEnemy : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.DefeatEnemy;
        questObjective = "적 격파";
        ValueUpRate = 5;
        objectiveValue = (questLevel + 1) / questCount < 1 ? 5 : (questLevel + 1) / questCount * ValueUpRate;
        currentValue = Manager.Data.Profile.Quest_Current_Progress;
        isClear = currentValue > objectiveValue;
    }

    public override void ObjectiveValueUp()
    {
        objectiveValue += ValueUpRate;
    }
}