using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UISummonBanner : UIBase
{
    #region Fields

    private SummonList _summonList;
    private SummonTable _summonTable;
    private UISubSceneShopSummon _shopSummon;
    private List<UIBtn_Check_Gems> _uiBtns = new();

    private TextMeshProUGUI _summonTypeText;
    private TextMeshProUGUI _summonLevel;
    private TextMeshProUGUI _summonCount;
    private Slider _summonGauge;

    #endregion

    #region Initialize

    public void ListInit(SummonList summonList, UISubSceneShopSummon shopSummon)
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
            var btnUI = GetUI<UIBtn_Check_Gems>(buttonInfo.BtnPrefab);
            Manager.Summon.SummonTables.TryGetValue(_summonList.TypeLink, out var summonTable);
            var button = SetButtonEvent(buttonInfo.BtnPrefab, UIEventType.Click, (eventData) => SummonBtn(eventData, btnUI));
            btnUI.SetButtonUI(buttonInfo, button, summonTable.SummonCountsAdd);
            _uiBtns.Add(btnUI);
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

    private void SummonBtn(PointerEventData eventData, UIBtn_Check_Gems btnUI)
    {
        if (!btnUI.Interactive) return;

        int addcount = 0;
        
        if (btnUI.ButtonInfo.OnEvent)
        {
            addcount = _summonTable.SummonCountsAdd;
        }

        _shopSummon.SummonTry(addcount, _summonList.TypeLink, btnUI);
    }

    #endregion

    #region UIUpdate Method

    public void UpdateUI()
    {
        _summonLevel.text = $"Lv {_summonTable.SummonGrade}";
        _summonCount.text = _summonTable.IsMaxGrade ? "MAX" : $"{_summonTable.GetCurCount}/{_summonTable.GetNextCount}";
        _summonGauge.value = _summonTable.IsMaxGrade ? 1 : Mathf.Clamp01((float)_summonTable.GetCurCount / (float)_summonTable.GetNextCount);
    }

    public void UpdateBtns(int summonCountsAdd)
    {
        for (int i = 0; i < _uiBtns.Count; i++)
        {
            _uiBtns[i].ApplySummonCountAdd(summonCountsAdd);
            _uiBtns[i].UpdateUI();
        }
    }
    
    #endregion
}
