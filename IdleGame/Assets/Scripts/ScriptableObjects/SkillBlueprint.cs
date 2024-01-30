using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Blueprints/Skill")]
public class SkillBlueprint : ScriptableObject
{
    [SerializeField] private string itemID;
    [SerializeField] private GameObject skillObject;

    public string ItemID => itemID;
    public GameObject SkillObject => skillObject;
}
