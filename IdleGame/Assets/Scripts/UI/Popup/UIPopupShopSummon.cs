using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private Player _player;
    private InventoryManager _inventoryManager;

    private Button _checkBtn;
    private Button _summonTryBtn_Ad;
    private Button _summonTryBtn_11;
    private Button _summonTryBtn_35;
    private Button _closeBtn;

    private int[] probability = new int[] { 5000, 2500, 1500, 700, 250, 50 };
    private int[] itemIndex = new int[] { 10001, 10002, 10003, 10005, 10006, 10007 };
    private Dictionary<int, int> probabilityTable = new Dictionary<int, int>();
    private List<int> summonResurt = new List<int>(1000);

    // 확인용
    private int[] testResult;
    private Dictionary<int, int> indexResult = new Dictionary<int, int>();

    #endregion

    #region Initialize

    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
        ProbabilityInit();

        _player = Manager.Game.Player;
        _inventoryManager = Manager.Inventory;
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        _summonTryBtn_Ad = SetButtonEvent("Btn_Summon_AdRepeat", UIEventType.Click, SummonRepeat_Ad);
        _summonTryBtn_11 = SetButtonEvent("Btn_Summon_11Repeat", UIEventType.Click, SummonRepeat_1);
        _summonTryBtn_35 = SetButtonEvent("Btn_Summon_35Repeat", UIEventType.Click, SummonRepeat_2);
        _closeBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
    }

    #endregion

    #region Button Events

    private void SummonRepeat_Ad(PointerEventData eventData)
    {
        SummonTry(0, 11);
    }

    private void SummonRepeat_1(PointerEventData eventData)
    {
        SummonTry(500, 11);
    }

    private void SummonRepeat_2(PointerEventData eventData)
    {
        SummonTry(1500, 35);
    }


    private void CloseSummonPopup(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }
    #endregion

    #region Summon Test

    private void ProbabilityInit()
    {
        int sum = 0;

        // 확인용 배열
        testResult = new int[probability.Length];

        // 확률 누계 딕셔너리 만들기
        for (int i = 0; i < probability.Length; i++)
        {
            sum += probability[i];
            probabilityTable.Add(sum, itemIndex[i]);
            // 확인용 배열 연결
            indexResult.Add(itemIndex[i], i);
        }
    }

    private void SummonTry(int price, int count)
    {
        if (_player.Gems < price)
        {
            Debug.Log("Not enough Gems");
            return;
        }
        Summon(count);
    }

    private void Summon(int count)
    {
        // 횟수만큼 랜덤값 뽑아서 배열로 만들고 리스트 비우기
        for (int i = 0; i < count; i++)
        {
            summonResurt.Add(Random.Range(0, 10000));
        }
        int[] summonResultValue = summonResurt.ToArray();
        summonResurt.Clear();

        // 딕셔너리 키만 뽑은 후 랜덤값보다 높은 숫자 중 가장 가까운 키를 찾아 인덱스 반환
        int[] summonValueKey = probabilityTable.Select(x => x.Key).ToArray();

        for (int i = 0; i < summonResultValue.Length; i++)
        {
            int getResultKey = summonValueKey.OrderBy(x => (summonResultValue[i] - x > 0)).First(); // 나중에 이진 탐색으로 줄여봅시다
            probabilityTable.TryGetValue(getResultKey, out int index);
            summonResurt.Add(index);
            // 확인용 획득 수 카운트 증가
            indexResult.TryGetValue(index, out int result);
            testResult[result]++;
        }
        TestDebugLog();

        int[] finalResult = summonResurt.ToArray();
        EquipmentAdd(finalResult);
        var popup = Manager.UI.ShowPopup<UIPopupDynamicRewardPopup>("DynamicRewardPopup");
        popup.DataInit(finalResult);
        popup.PlayStart();
        summonResurt.Clear();
    }

    private void EquipmentAdd(int[] summonResult)
    {

        for (int i = 0; i < summonResult.Length; i++)
        {
            ItemData itemData = _inventoryManager.SearchItem(summonResult[i]);
            itemData.hasCount++;
        }
        //_inventoryManager.SaveItemDataBase();
    }

    private void TestDebugLog()
    {
        Debug.Log($"{itemIndex[0]} : {testResult[0]}, {itemIndex[1]} : {testResult[1]}, {itemIndex[2]} : {testResult[2]}, {itemIndex[3]} : {testResult[3]}, {itemIndex[4]} : {testResult[4]}, {itemIndex[5]} : {testResult[5]}");
        for (int i = 0; i < testResult.Length; i++)
        {
            testResult[i] = 0;
        }
    }

    #endregion
}
