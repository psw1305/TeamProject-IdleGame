using Unity.VisualScripting;

public class QuestDamageUp : QuestData
{
    public override void Init(int questLevel, int questCount)
    {
        questType = QuestType.DamageUp;
        questObjective = "Damage Upgrade";
        ValueUpRate = 2;
        objectiveValue = (questLevel / questCount) < 1 ? 10 : (questLevel / questCount) * ValueUpRate * 10;
        currentValue = Manager.Data.Profile.Stat_Level_AtkDamage;
        isClear = currentValue > objectiveValue;
    }
}