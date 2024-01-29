using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UISummonBanner : UIBase
{
    #region Fields

    private SummonList _summonList;
    private UIPopupShopSummon _shopSummon;

    private Button _summonTryBtn_1;
    private Button _summonTryBtn_2;
    private Button _summonTryBtn_3;

    #endregion

    #region Initialize

    public void ListInit(SummonList summonList, UIPopupShopSummon shopSummon)
    {
        _summonList = summonList;
        _shopSummon = shopSummon;

        SetButtonEvents();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        _summonTryBtn_1 = SetButtonEvent("Btn_Summon_1", UIEventType.Click, SummonBtn_1);
        _summonTryBtn_2 = SetButtonEvent("Btn_Summon_2", UIEventType.Click, SummonBtn_2);
        _summonTryBtn_3 = SetButtonEvent("Btn_Summon_3", UIEventType.Click, SummonBtn_3);
    }

    #endregion

    #region Button Events
    private void SummonBtn_1(PointerEventData eventData)
    {
        _shopSummon.SummonTry(_summonList.PaymentType_1 ,_summonList.Amount_1, _summonList.SummonCount_1, _summonList.TableLink);
        Manager.NotificateDot.SetEquipmentNoti();
    }

    private void SummonBtn_2(PointerEventData eventData)
    {
        _shopSummon.SummonTry(_summonList.PaymentType_2, _summonList.Amount_2, _summonList.SummonCount_2, _summonList.TableLink);
        Manager.NotificateDot.SetEquipmentNoti();
    }

    private void SummonBtn_3(PointerEventData eventData)
    {
        _shopSummon.SummonTry(_summonList.PaymentType_3, _summonList.Amount_3, _summonList.SummonCount_3, _summonList.TableLink);
        Manager.NotificateDot.SetEquipmentNoti();
    }

    #endregion
}
