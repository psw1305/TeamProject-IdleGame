using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecommendWeaponItemSlotNotificate : BaseNotiDot
{
    ItemData currentItemData;
    protected override void Start()
    {
        base.Start();
        GetItemData();
    }

    private void GetItemData()
    {
        currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }
}
