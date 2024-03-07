public class QuestReachStage : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.StageClear;
        questObjective = "스테이지 클리어";
        ValueUpRate = 2;
        objectiveValue = (questLevel + 1) / questCount < 1 ? 2 : (questLevel + 1) / questCount * ValueUpRate;
        currentValue = Manager.Data.Profile.Stage_Chapter;
        isClear = currentValue > objectiveValue;
    }

    public override void ObjectiveValueUp()
    {
        objectiveValue += ValueUpRate;
    }
}
