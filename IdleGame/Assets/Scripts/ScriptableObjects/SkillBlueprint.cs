using UnityEngine;

public class SkillBlueprintt : ScriptableObject
{
    [SerializeField] private GameObject skillObject;

    public GameObject SkillObject => skillObject;
}
