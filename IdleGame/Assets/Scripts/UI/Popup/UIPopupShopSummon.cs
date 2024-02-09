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
    private SummonConfig _config;
    private Dictionary<string, UISummonBanner> _banners = new();
    private GameObject _content;

    #endregion

    #region Properties

    public SummonConfig Config => _config;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        _summon = Manager.Summon;
        _config = Manager.Assets.GetBlueprintSummon("SummonConfig") as SummonConfig;
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
        foreach (var list in _config.SummonLists)
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

    public void SummonTry(int addcount, string typeLink, UIBtn_Check_Gems btnUI)
    {
        _summon.SummonTry(addcount, typeLink, btnUI);
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

    public void BannerUpdate(string typeLink, int summonCountsAdd)
    {
        _banners.TryGetValue(typeLink, out var banner);
        banner.UpdateUI();
        banner.UpdateBtns(summonCountsAdd);
    }

    #endregion
}
