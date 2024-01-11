using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "Blueprints/Stage")]
public class StageBlueprint : ScriptableObject
{
    [SerializeField] private string[] enemies;
    [SerializeField] private int battleCount;

    public string[] Enemies => enemies;
    public int BattleCount => battleCount;
}
