using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UISubScenePlayerSystem : UIScene
{
    #region UI Fields

    #endregion

    #region Fields

    private Player player;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetButtonEvents();

        player = Manager.Game.Player;
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("Btn_Show_Weapon", UIEventType.Click, ShowPopupWeapon);
        SetButtonEvent("Btn_Show_Armor", UIEventType.Click, ShowPopupArmor);
        SetButtonEvent("Btn_Show_Skill", UIEventType.Click, ShowPopupSkill);
        SetButtonEvent("Btn_Show_Follower", UIEventType.Click, ShowPopupFollower);
        
        SetButtonEvent("Btn_Close", UIEventType.Click, CloseSubScene);
        SetButtonEvent("DimScreen", UIEventType.Click, CloseSubScene);
    }

    #endregion

    #region Button Events

    private void ShowPopupWeapon(PointerEventData eventData)
    {
        var equipmentPopup = Manager.UI.ShowPopup<UIPopupEquipment>();
        equipmentPopup.EquipFillterType = EquipFillterType.Weapon;
    }

    private void ShowPopupArmor(PointerEventData eventData)
    {
        var equipmentPopup = Manager.UI.ShowPopup<UIPopupEquipment>();
        equipmentPopup.EquipFillterType = EquipFillterType.Armor;
    }

    private void ShowPopupSkill(PointerEventData eventData) => Manager.UI.ShowPopup<UIPopupSkill>();
    private void ShowPopupFollower(PointerEventData eventData) => Manager.UI.ShowPopup<UIPopupFollower>();
    private void CloseSubScene(PointerEventData eventData) => Manager.UI.CloseSubScene();

    #endregion
}