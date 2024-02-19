using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupFollowerContainerInven : MonoBehaviour
{
    #region Fields

    public Dictionary<string, UIPopupFollowerSlotsInven> FollowerSlots = new Dictionary<string, UIPopupFollowerSlotsInven>();
    public GameObject itemInfoUI;
    private ScrollRect scrollRect;

    [SerializeField] private UIPopupFollower MainPopupUI;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    private void Start()
    {
        MainPopupUI = Manager.UI.CurrentPopup as UIPopupFollower;
        InitSlot();

        Manager.FollowerData.AddSetFollowerUIInvenSlot(SetUISlotFollowerEquipState);
        Manager.FollowerData.AddSetFollowerUIInvenSlot(SetUISlotFollowerReinforceState);
    }

    #endregion

    private void ResetOnScrollTop()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

    public void InitSlot()
    {
        FollowerSlots.Clear();

        foreach(var followerData in Manager.Data.FollowerData.UserInvenFollower)
        {
            UIPopupFollowerSlotsInven slot = Manager.Asset.InstantiatePrefab("Slot_Follower", gameObject.transform).GetComponent<UIPopupFollowerSlotsInven>();
            FollowerSlots.Add(followerData.itemID, slot);
            slot.InitSlotInfo(followerData);
            slot.InitSlotUI();
            slot.SetUIEquipState();
            slot.SetReinforceUI();
        }

        ResetOnScrollTop();
    }

    //슬롯에 장착 여부를 출력하기 위한 메서드
    public void SetUISlotFollowerEquipState()
    {
        foreach (var slot in FollowerSlots)
        {
            slot.Value.SetUIEquipState();
        }
    }

    public void SetUISlotFollowerEquipState(string id)
    {
        FollowerSlots[id].SetUIEquipState();
    }

    public void SetUISlotFollowerReinforceState()
    {
        foreach (var slot in FollowerSlots)
        {
            slot.Value.SetReinforceUI();
        }
    }

    public void SetUISlotFollowerReinforceState(string id)
    {
        FollowerSlots[id].SetReinforceData();
        FollowerSlots[id].SetReinforceIcon();
    }

    private void OnDestroy()
    {
        if (Manager.FollowerData != null)
        {
            Manager.FollowerData.SetFollowerUIInvenSlot -= SetUISlotFollowerEquipState;
            Manager.FollowerData.SetFollowerUIInvenSlot -= SetUISlotFollowerReinforceState;
        }
    }
}
