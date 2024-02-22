using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private Coroutine _autoSkillCoroutine;
    private Player _player;
    private Dictionary<int, EquipSkillData> _userEquipSkillSlot = new();
    public Dictionary<int, EquipSkillData> UserEquipSkillSlot => _userEquipSkillSlot;


    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private event Action<int> _skillUseAction;
    public void AddActionUseSkill(Action<int> skillUseEvent)
    {
        _skillUseAction += skillUseEvent;
    }
    public void RemoveActionUseSkill(Action<int> skillUseEvent)
    {
        _skillUseAction -= skillUseEvent;
    }

    private event Action<int> _skillChangeAction;
    public void AddActionChangeSkill(Action<int> skillChangeEvent)
    {
        _skillChangeAction += skillChangeEvent;
    }
    public void RemoveActionChangeSkill(Action<int> skillChangeEvent)
    {
        _skillChangeAction -= skillChangeEvent;
    }

    public void InitSkillSlot()
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

    public void ResetSkillCondition()
    {
        for (int i = 0; i < _userEquipSkillSlot.Count; i++)
        {
            _userEquipSkillSlot[i].SkillScript?.ResetSkill();
        }
    }

    public void ChangeEquipSkillData(int slotIndex)
    {
        _userEquipSkillSlot[slotIndex].SetSkillObject(Manager.Data.UserSkillData.UserEquipSkill[slotIndex].itemID);
        _skillChangeAction?.Invoke(slotIndex);
    }

    public bool ToggleAutoSkill(bool state)
    {
        if (state)
        {
            StopCoroutine(_autoSkillCoroutine);
            return false;
        }
        else
        {
            _autoSkillCoroutine = StartCoroutine(UseSkillLoop());
            return true;
        }
    }

    IEnumerator UseSkillLoop()
    {
        while (true)
        {

            yield return new WaitForSeconds(0.5f);

            if (_player.State != PlayerState.Move)
                for (int i = 0; i < _userEquipSkillSlot.Count; i++)
                {
                    if (_userEquipSkillSlot[i].SkillScript != null)
                    {
                        _userEquipSkillSlot[i].SkillScript.UseSkill();
                        _skillUseAction?.Invoke(i);
                    }
                }
        }
    }
}

public class EquipSkillData : MonoBehaviour
{
    public GameObject SkillObject { get; private set; }
    public BaseSkill SkillScript { get; private set; }
    public string SkillID { get; private set; }
    public void SetSkillObject(string itemID)
    {
        SkillID = itemID;

        if (itemID == "Empty")
        {
            if (SkillObject != null)
            {
                Destroy(SkillObject);
                SkillScript = null;
            }
            return;
        }

        // 슬롯에 스킬이 있으나 이미 프로퍼티가 설정되어 있는 경우 초기화
        if (SkillObject != null)
        {
            Destroy(SkillObject);
            SkillScript = null;
        }

        // 프로퍼티를 설정함
        var obj = Manager.SkillData.SkillDataDictionary[itemID].SkillObject;
        SkillObject = Instantiate(obj, Manager.Game.Player.transform);
        SkillObject.transform.parent = transform;
        SkillScript = SkillObject.GetComponent<BaseSkill>();
    }
}
