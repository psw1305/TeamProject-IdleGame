using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupFollowerContainer : UIBase
{
    public List<GameObject> itemSlots = new List<GameObject>();
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
        //InitSlot();
        //MainPopupUI.RefreshReinforecEvent += SetSlotEquipUI;
        //MainPopupUI.RefreshReinforecEvent += SetSlotReinforceUI;
    }

    private void ResetOnScrollTop()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

    public void InitSlot()
    {
        foreach (GameObject item in itemSlots)
        {
            Destroy(item);
        }

        itemSlots.Clear();
        foreach(var followerData in Manager.FollowerData.FollowerDataDictionary)
        {
            GameObject slot = Manager.Resource.InstantiatePrefab("Img_FollowerSlot", gameObject.transform);
            itemSlots.Add(slot);
            //slot.GetComponent<UIPopupEquipSlots>().InitSlotInfo(followerData);
        }
    }
}
