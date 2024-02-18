public class QuestDamageUp : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.DamageUp;
        questObjective = "공격력 강화";
        ValueUpRate = 10;
        objectiveValue = (questLevel / questCount) < 1 ? 10 : (questLevel + 1) / questCount * ValueUpRate;
        currentValue = Manager.Data.Profile.Stat_Level_AtkDamage;
        isClear = currentValue > objectiveValue;
    }

    public override void ObjectiveValueUp()
    {
        objectiveValue += ValueUpRate;
    }
}