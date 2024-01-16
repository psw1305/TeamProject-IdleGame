using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIPopupEquipContainer : MonoBehaviour
{
    private JsonParser invenJsonParser = new JsonParser();
    public List<GameObject> itemSlots = new List<GameObject>();
    private List<Button> SlotButton = new List<Button>();

    public GameObject itemInfoUI;

    private void Awake()
    {
        InitSlot();
    }

    private void InitSlot()
    {
        invenJsonParser.LoadItemDataBase();
        Debug.Log(invenJsonParser.itemDataBase.ItemDB.Count);
        foreach (var slots in invenJsonParser.itemDataBase.ItemDB)
        {
            GameObject slot = Manager.Resource.InstantiatePrefab("Img_ItemSlot", gameObject.transform);
            
            slot.GetComponent<UIPopupEquipSlots>().InitSlotInfo(slots);
            slot.GetComponent<UIPopupEquipSlots>().InitSlotUI();
        }
    }

}
