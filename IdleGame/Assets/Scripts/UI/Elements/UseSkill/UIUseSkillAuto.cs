using UnityEngine;
using UnityEngine.UI;

public class UIUseSkillAuto : MonoBehaviour
{
    private bool AutoState = false;
    private Button _button;
    private Image _image;
    private void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _button.onClick.AddListener(ChangeAutoSkillState);
        
        ChangeAutoSkillState();
    }

    private void ChangeAutoSkillState()
    {
        AutoState = Manager.Game.Player.gameObject.GetComponent<PlayerSkillHandler>().ToggleAutoSkill(AutoState);
        if (AutoState == true)
        {
            _image.color = Color.white;
        }
        else
        {
            _image.color = Color.gray;
        }
    }
}
