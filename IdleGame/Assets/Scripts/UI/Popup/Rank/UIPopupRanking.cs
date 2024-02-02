using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupRanking : UIPopup
{
    [SerializeField] private Transform contents;

    #region Initialize

    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
        SetRanking();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
    }

    #region Ranking Methods

    private void SetRanking()
    {
        Manager.Ranking.GetLeaderboard(contents);
    }

    #endregion

    #region Button Events

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    #endregion

    #endregion
}
