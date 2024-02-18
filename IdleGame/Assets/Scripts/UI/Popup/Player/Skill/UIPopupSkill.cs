using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupSkill : UIPopup
{
    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();

        SetButtonEvent("Btn_SkillSummon", UIEventType.Click, SummonPop);
        SetButtonEvent("Btn_SkillReinforceAll", UIEventType.Click, ReinforceAllSkill);

        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    private void ReinforceAllSkill(PointerEventData eventData)
    {
        Manager.SkillData.ReinforceAllSkill();
    }

    private void SummonPop(PointerEventData eventData)
    {
        //var SummonPopup = Manager.UI.ShowPopup<>();
    }
}
