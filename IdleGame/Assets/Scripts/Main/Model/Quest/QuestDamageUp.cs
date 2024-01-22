using Unity.VisualScripting;

public class QuestDamageUp : Quest, IQuestActive
{
    public override void Init()
    {
        data.questType = QuestType.DamageUp;
        data.questObjective = "Damage Up";
        //data.objectiveValue
        data.currentValue = Manager.Game.Player.AttackDamage.Level; // 플레이어 저장 데이터

    }

    public void ObjectiveValueUp()
    {

    }

    public void CurrentValueUp()
    {
        //데이터 값 증가 
    }
}