using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
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
    private Button _summonTryBtn_1;
    private Button _summonTryBtn_2;
    private Button _summonTryBtn_3;
    private Slider _summonGauge;

    #endregion

    #region Initialize

    public void ListInit(SummonList summonList, UIPopupShopSummon shopSummon)
    {
        _summonList = summonList;
        _shopSummon = shopSummon;
        Manager.Summon.SummonTable.TryGetValue(_summonList.TypeLink, out var summonTable);
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
        _summonTryBtn_1 = SetButtonEvent(_summonList.BtnPrefab_1, UIEventType.Click, SummonBtn_1);
        _summonTryBtn_2 = SetButtonEvent(_summonList.BtnPrefab_2, UIEventType.Click, SummonBtn_2);
        _summonTryBtn_3 = SetButtonEvent(_summonList.BtnPrefab_3, UIEventType.Click, SummonBtn_3);
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
    private void SummonBtn_1(PointerEventData eventData)
    {
        _shopSummon.SummonTry(_summonList.PaymentType_1 ,_summonList.Amount_1, _summonList.SummonCount_1, _summonList.TypeLink);
        Manager.NotificateDot.SetEquipmentNoti();
    }

    private void SummonBtn_2(PointerEventData eventData)
    {
        _shopSummon.SummonTry(_summonList.PaymentType_2, _summonList.Amount_2, _summonList.SummonCount_2, _summonList.TypeLink);
        Manager.NotificateDot.SetEquipmentNoti();
    }

    private void SummonBtn_3(PointerEventData eventData)
    {
        _shopSummon.SummonTry(_summonList.PaymentType_3, _summonList.Amount_3, _summonList.SummonCount_3, _summonList.TypeLink);
        Manager.NotificateDot.SetEquipmentNoti();
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
