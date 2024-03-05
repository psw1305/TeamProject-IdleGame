using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISubSceneShopSummon : UIScene
{
    #region Fields

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
        _config = Manager.Asset.GetBlueprint("SummonConfig") as SummonConfig;
        _content = transform.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        _summon.SetShopPopup(this);

        SummonBannerInit();
    }

    private void SummonBannerInit()
    {
        foreach (var list in _config.SummonLists)
        {
            var banner = Manager.Asset.InstantiatePrefab("SummonBanner").GetComponent<UISummonBanner>();
            _banners[list.TypeLink] = banner;
            banner.name = list.Banner.name;
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

    #endregion

    #region UI Update Method

    public void BannerUpdate(string typeLink, int summonCountsAdd)
    {
        _banners.TryGetValue(typeLink, out var banner);
        banner.UpdateUI();
        banner.UpdateBtns(summonCountsAdd);
    }

    #endregion
}
