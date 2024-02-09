using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Blueprints/Skill")]
public class SkillBlueprint : ScriptableObject
{
    [SerializeField] private string itemID;
    [SerializeField] private string skillName;
    [SerializeField] private GameObject skillObject;

    public string ItemID => itemID;
    public string SkillName => skillName;
    public GameObject SkillObject => skillObject;
}
