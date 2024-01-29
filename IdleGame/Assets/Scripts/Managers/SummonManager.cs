using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SummonManager
{
    #region Fields

    private string _jsonPath = $"{Application.dataPath}/Resources/Texts/SummonTable/EquipmentSummonTable.json";
    private string _tableText;
    private Player _player;
    private InventoryManager _inventoryManager;

    private int[] levelUpCount = new int[] { 0, 1000, 2000, 3000, 4000, -1 };

    // 확인용
    private int[] testResult;
    private string[] itemIndex;
    private Dictionary<string, int> indexResult = new();

    #endregion

    #region Initialize

    public void SetSummon()
    {
        _player = Manager.Game.Player;
        _inventoryManager = Manager.Inventory;
    }

    public void Initialize()
    {
        SummonLevel = 1;

        SummonLevelInitialize();
        ProbabilityInit();
    }

    #endregion

    #region Properties

    public int SummonLevel { get; private set; }
    public int SummonCounts { get; private set; }

    #endregion

    #region Summon

    public void SummonTry(int price, int count)
    {
        if (_player.IsTradeGems(price))
        {
            Summon(count);
        }
    }

    private void Summon(int count)
    {
        // 횟수만큼 랜덤값 뽑아서 배열로 만들고 리스트 비우기, 소환 횟수 증가
        for (int i = 0; i < count; i++)
        {
            summonResurt.Add(Random.Range(0, 10000));
        }
        int[] summonResultValue = summonResurt.ToArray();
        summonResurt.Clear();

        // 소환 레벨에서 딕셔너리 키(누적 확률)만 뽑은 후 랜덤값보다 높은 숫자 중 가장 가까운 키를 찾아 인덱스 반환
        probabilityTable.TryGetValue(SummonLevel, out var summonProbability);
        var curLevelTable = summonProbability;
        var curprobability = curLevelTable.Select(x => x.Key).ToArray();

        // 테스트 결과 확인용 배열 세팅
        testResult = new int[summonProbability.Count];
        itemIndex = summonProbability.Select(x => x.Value).ToArray();

        for (int i = 0; i < itemIndex.Length; i++)
        {
            indexResult[itemIndex[i]] = i;
        }

        int idx = 0;

        while (count > 0)
        {
            int getResultKey = curprobability.OrderBy(x => (summonResultValue[idx] - x >= 0)).First(); // 나중에 이진 탐색으로 줄여봅시다
            curLevelTable.TryGetValue(getResultKey, out string index);
            //Debug.Log($"idx : {idx}, summonResultValue : {summonResultValue[idx]}, getResultKey : {getResultKey}, index : {index}");
            resultIdList.Add(index);
            // 확인용 획득 수 카운트 증가
            indexResult.TryGetValue(index, out int result);
            testResult[result]++;
            count--;
            idx++;
            SummonCounts++;
            if (SummonCounts > levelUpCount[SummonLevel] && levelUpCount[SummonLevel] > 0)
            {
                SummonLevel++;
                probabilityTable.TryGetValue(SummonLevel, out var newSummonProbability);
                curLevelTable = newSummonProbability;
                curprobability = curLevelTable.Select(x => x.Key).ToArray();
            }
        }
        // 디버그용 테이블 체크하기
        //int[] getResultKeyArr = curLevelTable.Select(x => x.Key).ToArray();
        //string txtsum = string.Empty;
        //foreach (var item in getResultKeyArr)
        //{
        //    curLevelTable.TryGetValue(item, out int index);
        //    txtsum += $"{index}, ";
        //}
        //Debug.Log(txtsum);

        //TestDebugLog();

        // 최종 획득한 아이템 목록 배열 출력 후 인벤토리에 넣고 팝업 실행
        string[] finalResult = resultIdList.ToArray();
        EquipmentAdd(finalResult);
        var popup = Manager.UI.ShowPopup<UIPopupRewards>("UIPopupSummonRewards");
        popup.DataInit(finalResult);
        popup.PlayStart();
        summonResurt.Clear();
        resultIdList.Clear();
    }

    private void EquipmentAdd(string[] summonResult)
    {
        for (int i = 0; i < summonResult.Length; i++)
        {
            UserItemData itemData = _inventoryManager.SearchItem(summonResult[i]);
            itemData.hasCount++;
        }
        _inventoryManager.SaveUserItemData();
    }

    #endregion

    #region Debug Method

    private void DebugTableData()
    {
        foreach (var item in probabilityTable)
        {
            Debug.Log($"Level : {item.Key}");

            var cumulative = item.Value.Keys.ToArray();
            var itemId = item.Value.Values.ToArray();
            for (int i = 0; i < cumulative.Length; i++)
            {
                Debug.Log($"cumulative : {cumulative[i]}, itemId : {itemId[i]}");
            }
        }
    }

    private void TestDebugLog()
    {
        Debug.Log($"{itemIndex.Length}, {testResult.Length}");
        Debug.Log($"{itemIndex[0]} : {testResult[0]}, {itemIndex[1]} : {testResult[1]}, {itemIndex[2]} : {testResult[2]}, {itemIndex[3]} : {testResult[3]}, {itemIndex[4]} : {testResult[4]}, {itemIndex[5]} : {testResult[5]}");
        for (int i = 0; i < testResult.Length; i++)
        {
            testResult[i] = 0;
        }
    }

    #endregion
}


