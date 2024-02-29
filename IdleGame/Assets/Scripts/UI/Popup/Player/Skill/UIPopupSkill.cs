using UnityEngine;
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

        SetButtonEvent("Btn_SkillReinforceAll", UIEventType.Click, ReinforceAllSkill);
        SetButtonEvent("Btn_ShowSummon", UIEventType.Click, ShowSummonScene);
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    private void ReinforceAllSkill(PointerEventData eventData)
    {
        Manager.SkillData.ReinforceAllSkill();
        Manager.Notificate.SetPlayerStateNoti();
        Manager.Notificate.SetReinforceSkillNoti();
        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    private void ShowSummonScene(PointerEventData eventData)
    {
        Manager.UI.ShowSubScene<UISubSceneShopSummon>();
        Manager.UI.ClosePopup();
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
