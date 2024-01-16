using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIPopupEquipContainer : MonoBehaviour
{
    public List<GameObject> itemSlots = new List<GameObject>();
    public GameObject itemInfoUI;

    private void Awake()
    {
        InitSlot();
    }


    //JSON에서 파싱한 아이템 개수 만큼 슬롯을 생성합니다.
    private void InitSlot()
    {
        foreach (ItemData itemData in Manager.Inventory.itemDataBase.ItemDB)
        {
            GameObject slot = Manager.Resource.InstantiatePrefab("Img_ItemSlot", gameObject.transform);
            itemSlots.Add(slot);
            slot.GetComponent<UIPopupEquipSlots>().InitSlotInfo(itemData);
            slot.GetComponent<UIPopupEquipSlots>().InitSlotUI();
            slot.GetComponent<UIPopupEquipSlots>().CheckEquipState();
        }
    }
    //슬롯에 장착 여부를 춫력하기 위한 메서드
    public void SetSlotEquipUI()
    {
        foreach(var slot in itemSlots)
        {
            slot.GetComponent<UIPopupEquipSlots>().CheckEquipState();
        }
    }

}
