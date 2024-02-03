using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupFollowerContainerEquip : MonoBehaviour
{
    private List<UIPopupFollowerSlotsEquip> slots = new List<UIPopupFollowerSlotsEquip>();

    private void Start()
    {
        foreach (var slot in gameObject.GetComponentsInChildren<UIPopupFollowerSlotsEquip>())
        {
            slots.Add(slot);
        }
        InitChildSlot();
        Manager.FollowerData.AddSetFollowerUIEquipSlot(SetChildSlot);
    }

    private void InitChildSlot()
    {
        for (int i = 0; i < Manager.Data.FollowerData.UserEquipFollower.Count; i++)
        {
            if (Manager.Data.FollowerData.UserEquipFollower[i].itemID == "Empty")
                slots[i].SetSlotEmpty();
            else
                slots[i].SetSlotUI(Manager.Data.FollowerInvenDictionary[Manager.Data.FollowerData.UserEquipFollower[i].itemID]);
        }
    }
    /// <summary>
    /// 특정 인덱스의 동료 장착 슬롯 정보를 세팅합니다.
    /// </summary>
    /// <param name="index"></param>
    private void SetChildSlot(int? index)
    {
        if (index == null)
            return;
        if (Manager.Data.FollowerData.UserEquipFollower[index.Value].itemID == "Empty")
            slots[index.Value].SetSlotEmpty();
        else
            slots[index.Value].SetSlotUI(Manager.Data.FollowerInvenDictionary[Manager.Data.FollowerData.UserEquipFollower[index.Value].itemID]);
        Manager.Game.Player.gameObject.GetComponent<PlayerFollowerHandler>().ChangeEquipFollowerData(index.Value);
    }

    private void OnDestroy()
    {
        if (Manager.FollowerData != null)
        {
            Manager.FollowerData.RemoveSetFollowerUIEquipSlot(SetChildSlot);
        }
    }
}
