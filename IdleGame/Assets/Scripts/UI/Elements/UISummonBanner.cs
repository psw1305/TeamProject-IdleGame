using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UISummonBanner : UIBase
{
    #region Fields

    private SummonList _summonList;
    private SummonTable _summonTable;
    private UIPopupShopSummon _shopSummon;

    private TextMeshProUGUI _summonTypeText;
    private TextMeshProUGUI _summonLevel;
    private TextMeshProUGUI _summonCount;
    private Slider _summonGauge;

    #endregion

    #region Initialize

    public void ListInit(SummonList summonList, UIPopupShopSummon shopSummon)
    {
        _summonList = summonList;
        _shopSummon = shopSummon;
        Manager.Summon.SummonTables.TryGetValue(_summonList.TypeLink, out var summonTable);
        _summonTable = summonTable;

        SetButtonEvents();
        SetTexts();
        SetComponents();
        // 얘는 갱신될 필요가 없어서 여기로
        _summonTypeText.text = _summonList.SummonName;
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetUI<UIBtn_Check_Gems>();
        for (int i = 0; i < _summonList.ButtonInfo.Count; i++)
        {
            var buttonInfo = _summonList.ButtonInfo[i];
            var btnUI = GetUI<UIBtn_Check_Gems>(_summonList.ButtonInfo[i].BtnPrefab);
            var button = SetButtonEvent(_summonList.ButtonInfo[i].BtnPrefab, UIEventType.Click, (eventData) => SummonBtn(eventData, buttonInfo));
            btnUI.SetButtonUI(buttonInfo, button);
        }
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        _summonTypeText = GetUI<TextMeshProUGUI>("SummonTypeText");
        _summonLevel = GetUI<TextMeshProUGUI>("SummonLevel");
        _summonCount = GetUI<TextMeshProUGUI>("SummonCount");
    }

    private void SetComponents()
    {
        SetUI<Slider>();
        _summonGauge = GetUI<Slider>("SummonGauge");
    }

    #endregion

    #region Button Events
    private void SummonBtn(PointerEventData eventData, ButtonInfo buttonInfo)
    {
        int addcount = 0;

        if (buttonInfo.OnEvent)
        {
            addcount = _summonTable.SummonCountsAdd;
        }

        _shopSummon.SummonTry(buttonInfo.ResourceType, buttonInfo.Amount, buttonInfo.SummonCount + addcount, _summonList.TypeLink);
    }

    #endregion

    #region UIUpdate Method

    public void UpdateUI()
    {
        _summonLevel.text = $"Lv {_summonTable.SummonGrade}";
        _summonCount.text = $"{_summonTable.GetCurCount}/{_summonTable.GetNextCount}";
        _summonGauge.value = Mathf.Clamp01((float)_summonTable.GetCurCount / (float)_summonTable.GetNextCount);
    }
    
    #endregion
}
