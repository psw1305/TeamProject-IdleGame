using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificateRecommendArmorItemSlot : BaseNotiDot
{
    private UserItemData _currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.NotificateDot.SetRecommendArmorItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        _currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        var _recommendItem = Manager.NotificateDot.CheckRecommendItem(Manager.Inventory.ArmorItemList);
        if (_recommendItem == null || _recommendItem.equipped)
        {
            InactiveNotiDot();
            return;
        }
        if (_currentItemData == _recommendItem)
        {
            ActiveNotiDot();
        }
        else if (_currentItemData != _recommendItem)
        {
            InactiveNotiDot();
        }
    }

    private void OnDestroy()
    {
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.SetRecommendArmorItemNoti -= SetRecommendItemNoti;
        }
    }
}
