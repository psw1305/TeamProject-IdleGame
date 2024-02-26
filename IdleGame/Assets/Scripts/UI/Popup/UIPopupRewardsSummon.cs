using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupRewardsSummon : UIPopup
{
    #region Fields

    [SerializeField] private Transform itemContents;

    private SummonList summonList;
    private string[] itemData;
    private string itemType;
    private bool isSkip = false;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
        SetButtonEvent("CloseButton", UIEventType.Click, ClosePopup);
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
        StartCoroutine(ShowSlots());
    }

    private IEnumerator ShowSlots()
    {
        var rewardItemSlot = Manager.Asset.GetPrefab("ItemSlot_SummonReward");

        for (int i = 0; i < itemData.Length; i++)
        {
            if (!isSkip) yield return new WaitForSeconds(0.1f);

            var itemSlotClone = Instantiate(rewardItemSlot, itemContents).GetComponent<UIRewardsItemSlot>();
            itemSlotClone.UpdateSlot(itemType, itemData[i]);
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
            Manager.UI.ClosePopup();  
        }
    }

    #endregion
}
