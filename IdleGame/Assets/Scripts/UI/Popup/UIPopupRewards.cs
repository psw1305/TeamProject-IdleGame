using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupRewards : UIPopup
{
    #region Fields

    [SerializeField]private UIRewardsItemSlot[] _itemSlots;
    private GameObject _content;
    private GameObject _outbox;
    private Button _closeBtn;
    private string[] itemData;

    private bool _isSkip = false;

    #endregion

    #region Initialize

    private void Awake()
    {
        SlotInit();
    }

    protected override void Init()
    {
        base.Init();
        ButtonActionInit();
    }

    private void ButtonActionInit()
    {
        SetUI<Button>();
        _closeBtn = SetButtonEvent("CloseButton", UIEventType.Click, ClosePopup);
    }

    private void SlotInit()
    {
        _itemSlots = transform.GetComponentsInChildren<UIRewardsItemSlot>(true);
        _content = transform.GetComponentInChildren<GridLayoutGroup>().gameObject;
        _outbox = transform.Find("_Outbox").gameObject;
    }

    private void SlotClear()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].transform.SetParent(_outbox.transform, false);
        }
    }

    private void SlotSetting()
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            _itemSlots[i].transform.SetParent(_content.transform, false);
        }
    }

    public void DataInit(string[] itemDatas)
    {
        itemData = itemDatas;
    }

    #endregion

    #region Popup Actions

    public void PlayStart()
    {
        SlotSetting();
        StartCoroutine(ShowSlots());
    }

    private IEnumerator ShowSlots()
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            if (!_isSkip)
                yield return new WaitForSeconds(0.05f);
            _itemSlots[i].gameObject.SetActive(true);
            _itemSlots[i].UpdateSlot(itemData[i]);
        }
        _isSkip = true;
    }

    #endregion

    #region Button Events

    private void ClosePopup(PointerEventData eventData)
    {
        if (!_isSkip)
        {
            _isSkip = true;
        }
        else
        {
            SlotClear();
            Manager.UI.ClosePopup();  
        }
    }

    #endregion
}
