public class QuestReachStage : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.StageClear;
        questObjective = "Reach Staege";
        ValueUpRate = 2;
        objectiveValue = ((questLevel / questCount) + 1)  * ValueUpRate;
        currentValue = Manager.Data.Profile.Stage_Level;
        isClear = currentValue > objectiveValue;
    }
}
