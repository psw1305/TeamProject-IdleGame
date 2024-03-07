using UnityEngine;

[CreateAssetMenu(fileName = "PoolableBlueprint", menuName = "Blueprints/PoolableBlueprint")]
public class PoolableBlueprint : ScriptableObject
{
    [Header("Poolable Info")]
    [SerializeField] private string poolableName;
    [SerializeField] private GameObject poolableObject;

    public string PoolableName => poolableName;
    public GameObject PoolableObject => poolableObject;
}
