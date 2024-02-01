using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private bool _autoSkill;
    private Coroutine _autoSkillCoroutine;

    private Dictionary<SkillSlots, SkillObject> _userEquipSkillSlot = new Dictionary<SkillSlots, SkillObject>();

    private void Start()
    {
        InitSkillSlot();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            ToggleAutoSkill();
        }
    }

    private void InitSkillSlot()
    {
        int slotIndex = 0;
        foreach (var item in Manager.Data.UserSkillData.UserEquipSkill)
        {
            if (item.itemID != "Empty")
            {
                SetSkillObj((SkillSlots)slotIndex, new SkillObject(item.itemID));
            }
            slotIndex++;
        }
    }


    private void SetSkillObj(SkillSlots enumSkillSlots, SkillObject skillObject)
    {
        _userEquipSkillSlot.Add(enumSkillSlots, skillObject);
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
            foreach (var skill in _userEquipSkillSlot)
            {
                skill.Value._baseSkill.UseSkill();
            }
            Debug.Log("루프중");
        }
    }
}

public class SkillObject
{
    public GameObject _skillObject { get; private set; }
    public BaseSkill _baseSkill { get; private set; }
    public SkillObject(string itemID)
    {
        _skillObject = Manager.Resource.InstantiatePrefab((Manager.Resource.GetBlueprint(itemID) as SkillBlueprint).SkillObject.name
            , Manager.Game.Player.transform);
        _baseSkill = _skillObject.GetComponent<BaseSkill>();
    }
}
