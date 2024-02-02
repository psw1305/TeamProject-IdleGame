public class QuestReachStage : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.StageClear;
        questObjective = "스테이지 클리어";
        ValueUpRate = 2;
        objectiveValue = ((questLevel / questCount) + 1)  * ValueUpRate;
        currentValue = Manager.Data.Profile.Stage_Chapter;
        isClear = currentValue > objectiveValue;
    }
}
