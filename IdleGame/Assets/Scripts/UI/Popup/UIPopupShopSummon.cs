using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine;
using System.Collections.Generic;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private SummonManager _summon;
    private Dictionary<string, UISummonBanner> _banners = new Dictionary<string, UISummonBanner>();

    private Button _closeBtn;
    private GameObject _content;

    private SummonBlueprint _summonBlueprint;

    #endregion

    #region Properties

    public SummonBlueprint SummonBlueprint => _summonBlueprint;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        _summon = Manager.Summon;
        _summonBlueprint = Manager.Resource.GetBlueprint("SummonConfig") as SummonBlueprint;
        _content = transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        _summon.SetShopPopup(this);

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
            _banners[list.TypeLink] = banner;
            banner.ListInit(list, this);
            banner.transform.SetParent(_content.transform, false);
            banner.UpdateUI();
        }
    }

    #endregion

    #region Button Events

    public void SummonTry(ResourceType type,int price, int count, string typeLink)
    {
        _summon.SummonTry(type, price, count, typeLink);
    }

    private void CloseSummonPopup(PointerEventData eventData)
    {
        _summon.SetShopPopup(null);
        Manager.UI.ClosePopup();
    }
    #endregion

    #region UIUpdate Method

    public void BannerUpdate(string typeLink)
    {
        _banners.TryGetValue(typeLink, out var banner);
        banner.UpdateUI();
    }

    #endregion
}
