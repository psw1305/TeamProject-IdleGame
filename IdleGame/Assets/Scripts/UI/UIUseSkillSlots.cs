using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIUseSkillSlots : MonoBehaviour
{
    [SerializeField] private Image ImgSkillIcon;
    [SerializeField] private Image ImgDurate;
    [SerializeField] private Image ImgCoolDown;
    private EquipSkillData _equipSkillData;


    public void SetUISkillSlot(EquipSkillData equipSkillData)
    {
        _equipSkillData = equipSkillData;
        ImgDurate.fillAmount = 0;
        ImgCoolDown.fillAmount = 0;
        StopAllCoroutines();

        if (_equipSkillData.SkillID == "Empty")
        {
            ImgSkillIcon.gameObject.SetActive(false);
            return;
        }

        else
        {
            ImgSkillIcon.sprite = Manager.Resource.GetSprite(_equipSkillData.SkillID);
            ImgSkillIcon.gameObject.SetActive(true);
            
            StartCoroutine(SetUIDurateTime());
        }
    }

    public void SetUIUseSkill()
    {
        StartCoroutine(SetUIDurateTime());
    }

    IEnumerator SetUIDurateTime()
    {
        while (_equipSkillData.SkillScript.CurrentDurateTime != 0)
        {
            yield return new WaitForSeconds(0.1f);
            ImgDurate.fillAmount = _equipSkillData.SkillScript.CurrentDurateTime / _equipSkillData.SkillScript.EffectDurateTime;
        }
        ImgDurate.fillAmount = 0f;

        StartCoroutine(SetUICoolDown());
    }

    IEnumerator SetUICoolDown()
    {
        while (_equipSkillData.SkillScript.CurrentCoolDown != 0)
        {
            yield return new WaitForSeconds(0.1f);
            ImgCoolDown.fillAmount = _equipSkillData.SkillScript.CurrentCoolDown / _equipSkillData.SkillScript.CoolDown;
        }
        ImgCoolDown.fillAmount = 0f;
    }
}
