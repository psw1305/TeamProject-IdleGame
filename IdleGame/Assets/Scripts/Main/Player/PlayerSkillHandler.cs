using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private bool _autoSkill;
    private Coroutine _autoSkillCoroutine;

    private Dictionary<int, EquipSkillData> _userEquipSkillSlot = new Dictionary<int, EquipSkillData>();

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
        int equipslotIndex = 0;
        foreach (var item in Manager.Data.UserSkillData.UserEquipSkill)
        {
            var go = new GameObject("SkillObj");
            go.transform.parent = transform;
            _userEquipSkillSlot.Add(equipslotIndex, go.AddComponent<EquipSkillData>());
            _userEquipSkillSlot[equipslotIndex].SetSkillObject(Manager.Data.UserSkillData.UserEquipSkill[equipslotIndex].itemID);
            equipslotIndex++;
        }
    }

    public void ChangeEquipSkillData(int slotIndex)
    {
        _userEquipSkillSlot[slotIndex].SetSkillObject(Manager.Data.UserSkillData.UserEquipSkill[slotIndex].itemID);
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
                if (skill.Value.SkillScript != null)
                {
                    skill.Value.SkillScript.UseSkill();
                }
            }
        }
    }
}

public class EquipSkillData : MonoBehaviour
{
    public GameObject SkillObject { get; private set; }
    public BaseSkill SkillScript { get; private set; }

    private string _skillID;
    public void SetSkillObject(string itemID)
    {
        if (itemID == "Empty")
        {
            if (SkillObject != null)
            {
                Destroy(SkillObject);
                SkillScript = null;
            }
            return;
        }

        //슬롯에 스킬이 있으나 이미 프로퍼티가 설정되어 있는 경우 초기화
        if (SkillObject != null)
        {
            Destroy(SkillObject);
            SkillScript = null;
        }
        //프로퍼티를 설정함
        SkillObject = Manager.Resource.InstantiatePrefab((Manager.Resource.GetBlueprint(itemID) as SkillBlueprint).SkillObject.name, Manager.Game.Player.transform);
        SkillObject.transform.parent = transform;
        SkillScript = SkillObject.GetComponent<BaseSkill>();
    }
}
