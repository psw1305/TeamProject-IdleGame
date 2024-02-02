public class QuestDamageUp : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.DamageUp;
        questObjective = "공격력 강화";
        ValueUpRate = 2;
        objectiveValue = (questLevel / questCount) < 1 ? 10 : (questLevel / questCount) * ValueUpRate * 10;
        currentValue = Manager.Data.Profile.Stat_Level_AtkDamage;
        isClear = currentValue > objectiveValue;
    }
}