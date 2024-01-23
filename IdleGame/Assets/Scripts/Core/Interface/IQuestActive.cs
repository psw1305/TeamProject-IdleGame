using UnityEngine;

public interface IQuestActive
{
    void Init(int QuestLevel, int QuestCount);

    void ObjectiveValueUp(int QuestLevel, int QuestCount);
}
