using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIUseSkillSlots : MonoBehaviour
{
    [SerializeField] private Image SkillIcon;
    [SerializeField] private Image Durate;
    [SerializeField] private Image CoolDown;
    private EquipSkillData _equipSkillData;

    public EquipSkillData EquipSkillData => _equipSkillData;
    public void SetUISkillSlot()
    {
        
    }
    private void Start()
    {
        
    }
}
