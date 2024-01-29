using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private SummonManager _summon;

    private Button _closeBtn;
    private GameObject _content;

    private SummonBlueprint _summonBlueprint;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        _summon = Manager.Summon;
        _summonBlueprint = Manager.Resource.GetBlueprint("SummonConfig") as SummonBlueprint;
        _content = transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

        SetButtonEvents();
        SummonBannerInit();
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        _closeBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
    }

    private void SummonBannerInit()
    {
        foreach (var list in _summonBlueprint.SummonLists)
        {
            var banner = Manager.UI.AddElement<UISummonBanner>(list.Banner.name);
            banner.ListInit(list, this);
            banner.transform.SetParent(_content.transform, false);
        }
    }

    #endregion

    #region Button Events

    public void SummonTry(ResourceType type,int price, int count, string tableLink)
    {
        _summon.SummonTry(type, price, count, tableLink);
    }

    private void CloseSummonPopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }
    #endregion
}
