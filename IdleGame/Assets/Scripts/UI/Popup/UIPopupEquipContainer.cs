using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UIPopupEquipContainer : UIBase
{
    public List<GameObject> itemSlots = new List<GameObject>();
    public GameObject itemInfoUI;
    private ScrollRect scrollRect;

    [SerializeField] private UIPopupEquipment MainPopupUI;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    private void Start()
    {
        MainPopupUI = Manager.UI.CurrentPopup as UIPopupEquipment;
        InitSlot();
        MainPopupUI.refreshReinforecEvent += SetSlotEquipUI;
        MainPopupUI.refreshReinforecEvent += SetSlotReinforceUI;
    }
    private void ResetOnScrollTop()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }

    //JSON에서 파싱한 아이템 개수 만큼 슬롯을 생성합니다.
    public void InitSlot()
    {
        foreach (GameObject item in itemSlots)
        {
            Destroy(item);
        }

        itemSlots.Clear();

        foreach (var itemData in MainPopupUI.FillterItems)
        {
            GameObject slot = Manager.Resource.InstantiatePrefab("Img_ItemSlot", gameObject.transform);
            itemSlots.Add(slot);
            slot.GetComponent<UIPopupEquipSlots>().InitSlotInfo(itemData);
            slot.GetComponent<UIPopupEquipSlots>().InitSlotUI();
            slot.GetComponent<UIPopupEquipSlots>().CheckEquipState();
            slot.GetComponent<UIPopupEquipSlots>().SetReinforceUI();
        }
        ResetOnScrollTop();
    }

    //슬롯에 장착 여부를 춫력하기 위한 메서드
    public void SetSlotEquipUI()
    {
        foreach (var slot in itemSlots)
        {
            slot.GetComponent<UIPopupEquipSlots>().CheckEquipState();
        }
    }
    public void SetSlotReinforceUI()
    {
        foreach (var slot in itemSlots)
        {
            slot.GetComponent<UIPopupEquipSlots>().SetReinforceUI();
        }
    }
}
