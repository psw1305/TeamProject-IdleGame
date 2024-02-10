using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "Blueprints/StageBlueprint")]
public class StageBlueprint : ScriptableObject
{
    [Header("Monster Data")]
    [SerializeField] private string boss;
    [SerializeField] private string[] enemies;

    [Space(20)]
    [SerializeField] private int battleCount;

    public string[] Enemies => enemies;
    public string Boss => boss;
    public int BattleCount => battleCount;
}
