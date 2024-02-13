using UnityEngine;

[CreateAssetMenu(fileName = "SkillBlueprint", menuName = "Blueprints/SkillBlueprint")]
public class SkillBlueprint : ScriptableObject
{
    [SerializeField] private GameObject skillObject;

    public GameObject SkillObject => skillObject;
}
