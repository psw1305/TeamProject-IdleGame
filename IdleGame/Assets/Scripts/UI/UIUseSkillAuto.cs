using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUseSkillAuto : MonoBehaviour
{
    private bool AutoState = false;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ChangeAutoSkillState);
        ChangeAutoSkillState();
    }

    private void ChangeAutoSkillState()
    {
        Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ToggleAutoSkill(AutoState);
        gameObject.GetComponent<Button>().interactable = !AutoState;
    }
}
