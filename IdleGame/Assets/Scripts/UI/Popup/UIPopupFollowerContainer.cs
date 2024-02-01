using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupFollowerContainer : UIBase
{
    public List<GameObject> FollowerSlots = new List<GameObject>();
    public GameObject itemInfoUI;
    private ScrollRect scrollRect;
    [SerializeField] private UIPopupFollower MainPopupUI;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    private void Start()
    {
        MainPopupUI = Manager.UI.CurrentPopup as UIPopupFollower;
        InitSlot();
        MainPopupUI.RefreshReinforecEvent += SetFollowerSlotUI;
        MainPopupUI.RefreshReinforecEvent += SetFollowerSlotReinforceUI;
    }

    private void ResetOnScrollTop()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

    public void InitSlot()
    {
        FollowerSlots.Clear();

        foreach(var followerData in Manager.Data.FollowerData.UserInvenFollower)
        {
            GameObject slot = Manager.Resource.InstantiatePrefab("Img_FollowerSlot", gameObject.transform);
            FollowerSlots.Add(slot);
            slot.GetComponent<UIPopupFollowerSlots>().InitSlotInfo(followerData);
            slot.GetComponent<UIPopupFollowerSlots>().InitSlotUI();
            slot.GetComponent<UIPopupFollowerSlots>().CheckEquipState();
            slot.GetComponent<UIPopupFollowerSlots>().SetReinforceUI();
        }

        ResetOnScrollTop();
    }

    //슬롯에 장착 여부를 출력하기 위한 메서드
    public void SetFollowerSlotUI()
    {
        foreach (var slot in FollowerSlots)
        {
            slot.GetComponent<UIPopupFollowerSlots>().CheckEquipState();
        }
    }
    public void SetFollowerSlotReinforceUI()
    {
        foreach (var slot in FollowerSlots)
        {
            slot.GetComponent<UIPopupFollowerSlots>().SetReinforceUI();
        }
    }
}
