public class QuestDefeatEnemy : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.DefeatEnemy;
        questObjective = "적 격파";
        ValueUpRate = 1;
        objectiveValue = 5;
        currentValue = 0;
        isClear = currentValue > objectiveValue;
    }
}