using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "Blueprints/Stage")]
public class StageBlueprint : ScriptableObject
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private int battleCount;

    public Enemy[] Enemies => enemies;
    public int BattleCount => battleCount;
}
