using System.Collections.Generic;
using UnityEngine;

public class UIPopupFollowerContainerEquip : MonoBehaviour
{
    private List<UIPopupFollowerSlotsEquip> _slots = new List<UIPopupFollowerSlotsEquip>();

    [SerializeField] private GameObject _DimCover;

    private void Start()
    {
        foreach (var slot in gameObject.GetComponentsInChildren<UIPopupFollowerSlotsEquip>())
        {
            _slots.Add(slot);
        }
        InitChildSlot();
        Manager.FollowerData.AddSetFollowerUIEquipSlot(SetChildSlot);
    }

    private void InitChildSlot()
    {
        for (int i = 0; i < Manager.Data.FollowerData.UserEquipFollower.Count; i++)
        {
            if (Manager.Data.FollowerData.UserEquipFollower[i].itemID == "Empty")
                _slots[i].SetSlotEmpty();
            else
                _slots[i].SetSlotUI(Manager.Data.FollowerInvenDictionary[Manager.Data.FollowerData.UserEquipFollower[i].itemID]);
        }
    }
    /// <summary>
    /// 특정 인덱스의 동료 장착 슬롯 정보를 세팅합니다.
    /// </summary>
    /// <param name="index"></param>
    private void SetChildSlot(int index)
    {
        if (index == -100)
        {
            return;
        }
        if(index == -200)
        {
            ToggleSlotReplaceMode();
            return;
        }

        if (Manager.Data.FollowerData.UserEquipFollower[index].itemID == "Empty")
            _slots[index].SetSlotEmpty();
        else
            _slots[index].SetSlotUI(Manager.Data.FollowerInvenDictionary[Manager.Data.FollowerData.UserEquipFollower[index].itemID]);
        Manager.Game.Player.gameObject.GetComponent<PlayerFollowerHandler>().ChangeEquipFollowerData(index);
    }

    public void ToggleSlotReplaceMode()
    {
        foreach (var slot in _slots)
        {
            slot.ReplaceMode = !slot.ReplaceMode;
        }
        _DimCover.gameObject.SetActive(!_DimCover.gameObject.activeSelf);
    }

    private void OnDestroy()
    {
        if (Manager.FollowerData != null)
        {
            Manager.FollowerData.RemoveSetFollowerUIEquipSlot(SetChildSlot);
        }
    }
}
