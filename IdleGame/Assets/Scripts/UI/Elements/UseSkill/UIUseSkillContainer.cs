using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUseSkillContainer : MonoBehaviour
{
    private List<UIUseSkillSlots> _slots = new();
    private PlayerSkillHandler _playerSkillHandler;

    private void Start()
    {
        _playerSkillHandler = Manager.Game.Player.GetComponent<PlayerSkillHandler>();

        for (int i = 0; i < gameObject.GetComponentsInChildren<UIUseSkillSlots>().Length; i++)
        {
            _slots.Add(gameObject.GetComponentsInChildren<UIUseSkillSlots>()[i]);
            _slots[i].SetUISkillSlot(_playerSkillHandler.UserEquipSkillSlot[i]);
        }
        _playerSkillHandler.AddActionUseSkill(SetUISkillState);
        _playerSkillHandler.AddActionChangeSkill(SetSkillUIState);
    }

    public void SetSkillUIState(int index)
    {
        _slots[index].SetUISkillSlot(_playerSkillHandler.UserEquipSkillSlot[index]);
    }


    public void SetUISkillState(int index)
    {
        _slots[index].SetUIUseSkill();
    }

    private void OnDestroy()
    {
        if(_playerSkillHandler != null)
        {
            _playerSkillHandler.RemoveActionUseSkill(SetUISkillState);
            _playerSkillHandler.RemoveActionChangeSkill(SetSkillUIState);
        }
    }
}
