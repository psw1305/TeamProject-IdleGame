using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupRewards : UIPopup
{
    #region Fields

    [SerializeField] private UIRewardsItemSlot[] itemSlots;
    [SerializeField] private GameObject outbox;

    private GameObject content;
    private SummonList summonList;
    private string[] itemData;
    private string itemType;

    private bool isSkip = false;

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
        SetButtonEvent("CloseButton", UIEventType.Click, ClosePopup);
    }

    private void SlotInit()
    {
        itemSlots = transform.GetComponentsInChildren<UIRewardsItemSlot>(true);
        content = transform.GetComponentInChildren<GridLayoutGroup>().gameObject;
    }

    private void SlotClear()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].transform.SetParent(outbox.transform, false);
        }
    }

    private void SlotSetting()
    {
        for (int i = 0; i < itemData.Length; i++)
        {
            itemSlots[i].transform.SetParent(content.transform, false);
        }
    }

    public void DataInit(string typeLink, string[] itemDatas)
    {
        itemType = typeLink;
        itemData = itemDatas;
    }

    public void SummonButtonInit(SummonList summonList)
    {
        this.summonList = Manager.Asset.GetBlueprint("SummonRewards") as SummonList;
        SetUI<Button>();
        SetUI<UIBtn_Check_Gems>();
        for (int i = 0; i < this.summonList.ButtonInfo.Count; i++)
        {
            var buttonInfo = this.summonList.ButtonInfo[i];
            var sourceButtonInfo = summonList.ButtonInfo.Find(x => x.BtnPrefab == buttonInfo.BtnPrefab);
            // 이름이 같은 buttoninfo가 있으면 그걸로 적용
            if (summonList.ButtonInfo.Contains(sourceButtonInfo))
            {
                buttonInfo = sourceButtonInfo;
            }
            // 아닐 경우 원본에서
            var btnUI = GetUI<UIBtn_Check_Gems>(buttonInfo.BtnPrefab);
            Manager.Summon.SummonTables.TryGetValue(summonList.TypeLink, out var summonTable);
            // TODO : 창 닫히는것 까지 연결하기
            var button = SetButtonEvent(buttonInfo.BtnPrefab, UIEventType.Click, ClosePopup);
            button = SetButtonEvent(buttonInfo.BtnPrefab, UIEventType.Click, (eventdata) => Manager.Summon.SummonTry(0, summonList.TypeLink, btnUI));
            btnUI.SetButtonUI(buttonInfo, button, summonTable.SummonCountsAdd);
        }

        // 반복 버튼 연결
        SetButtonEvent("Btn_Summon_Repeat", UIEventType.Click, ClosePopup);
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
            if (!isSkip) yield return new WaitForSeconds(0.05f);

            itemSlots[i].gameObject.SetActive(true);
            itemSlots[i].UpdateSlot(itemType, itemData[i]);
        }

        isSkip = true;
    }

    #endregion

    #region Button Events

    private void ClosePopup(PointerEventData eventData)
    {
        if (!isSkip)
        {
            isSkip = true;
        }
        else
        {
            SlotClear();
            Manager.UI.ClosePopup();  
        }
    }

    #endregion
}
