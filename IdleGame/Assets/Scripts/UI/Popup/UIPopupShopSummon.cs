using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private TextMeshProUGUI goldText;
    private TextMeshProUGUI gemsText;

    private Player player;
    private SummonManager _summon;
    private SummonBlueprint _summonBlueprint;
    private Dictionary<string, UISummonBanner> _banners = new();
    private GameObject _content;

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

        SetTexts();
        SetButtonEvents();
        SummonBannerInit();
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        goldText = GetUI<TextMeshProUGUI>("Txt_Gold");
        gemsText = GetUI<TextMeshProUGUI>("Txt_Jewel");

        UpdateGold();
        UpdateGems();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("Btn_Close", UIEventType.Click, CloseSummonPopup);
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

    #region UI Update Method

    public void UpdateGold()
    {
        goldText.text = Manager.Game.Player.Gold.ToString();
    }

    public void UpdateGems()
    {
        gemsText.text = Manager.Game.Player.Gems.ToString();
    }

    public void BannerUpdate(string typeLink)
    {
        _banners.TryGetValue(typeLink, out var banner);
        banner.UpdateUI();
    }

    #endregion
}
