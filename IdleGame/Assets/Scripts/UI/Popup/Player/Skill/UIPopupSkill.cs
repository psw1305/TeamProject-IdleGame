using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupSkill : UIPopup
{
    private Image _popupDimCover;

    protected override void Init()
    {
        base.Init();
        SetImage();
        SetButtonEvents();
    }

    private void SetImage()
    {
        SetUI<Image>();

        _popupDimCover = GetUI<Image>("Img_PopupDimCover");
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

        Manager.Game.Player.EquipmentStatModifier();
        (Manager.UI.CurrentScene as UISceneMain).UpdatePlayerPower();
    }

    private void ShowSummonScene(PointerEventData eventData)
    {
        Manager.UI.ShowSubScene<UISubSceneShopSummon>();
        Manager.UI.ClosePopup();
    }

    public void TogglePopupDim()
    {
        _popupDimCover.enabled = !_popupDimCover.enabled;
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
}
