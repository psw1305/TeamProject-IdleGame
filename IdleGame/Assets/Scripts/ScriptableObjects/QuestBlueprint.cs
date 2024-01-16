using UnityEngine;

[CreateAssetMenu(fileName = "Quest_", menuName = "Blueprints/Quest")]
public class QuestBlueprint : ScriptableObject
{
    [Header("Quest Data")]
    [SerializeField] private QuestType questType;
    [SerializeField] private string questObjective;  // 퀘스트 내용
    [SerializeField] private int objectiveValue;

    public QuestType QuestType => questType;
    public string QuestObjective => questObjective;
    public int ObjectiveValue => objectiveValue;

}
