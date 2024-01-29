using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private bool _autoSkill;
    private Coroutine _autoSkillCoroutine;

    public List<SkillBlueprint> SkillObjectList = new List<SkillBlueprint>();
    public List<BaseSkill> baseSkills = new List<BaseSkill>();

    private void Start()
    {
        SetSkillObj();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            ToggleAutoSkill();
        }
    }

    private void SetSkillObj()
    {
        foreach (var skill in SkillObjectList)
        {
            baseSkills.Add(Instantiate(skill.SkillObject, gameObject.transform).GetComponent<BaseSkill>());
        }
    }

    private void ToggleAutoSkill()
    {
        if (_autoSkill)
        {
            _autoSkill = false;
            StopCoroutine(_autoSkillCoroutine);
            Debug.Log("toggleOff");
        }
        else if (!_autoSkill)
        {
            _autoSkill = true;
            _autoSkillCoroutine = StartCoroutine(UseSkillLoop());
            Debug.Log("toggleON");
        }
    }

    IEnumerator UseSkillLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var skill in baseSkills)
            {
                skill.UseSkill();
            }
            Debug.Log("루프중");
        }
    }
}
