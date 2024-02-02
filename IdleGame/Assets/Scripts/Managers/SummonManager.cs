using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SummonManager
{
    #region Fields

    private Player _player;
    private InventoryManager _inventoryManager;
    private UIPopupShopSummon _shopSummon;

    private List<int> summonResurt = new List<int>(200);
    private List<string> resultIdList = new List<string>(200);

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
        _summonBlueprint = Manager.Resource.GetBlueprint("SummonConfig") as SummonBlueprint;

        foreach (var list in _summonBlueprint.SummonLists)
        {
            TableInitalize(list);
        }
    }

    public void SetShopPopup(UIPopupShopSummon uIPopupShopSummon)
    {
        _shopSummon = uIPopupShopSummon;
    }

    #endregion

    #region Summon

    public void SummonTry(ResourceType type,int price, int count, string tableLink)
    {
        switch (type)
        {
            case ResourceType.Gold:
                if (_player.IsTradeGold(price))
                    Summon(count, tableLink);
                break;
            case ResourceType.Gems:
                if (_player.IsTradeGems(price))
                    Summon(count, tableLink);
                break;
        }
    }

    private void Summon(int count, string typeLink)
    {
        // 횟수만큼 랜덤값 뽑아서 배열로 만들고 리스트 비우기, 소환 횟수 증가
        for (int i = 0; i < count; i++)
        {
            summonResurt.Add(Random.Range(0, 10000));
        }
        int[] summonResultValue = summonResurt.ToArray();
        summonResurt.Clear();

        // 소환 레벨에서 딕셔너리 키(누적 확률)만 뽑은 후 랜덤값보다 높은 숫자 중 가장 가까운 키를 찾아 인덱스 반환
        SummonTables.TryGetValue(typeLink, out var summonTable);
        var curLevelTable = summonTable.GetProbabilityTable();
        var curprobability = curLevelTable.Select(x => x.Key).ToArray();

        // 테스트 결과 확인용 배열 세팅
        //testResult = new int[curLevelTable.Count];
        //itemIndex = curLevelTable.Select(x => x.Value).ToArray();

        //for (int i = 0; i < itemIndex.Length; i++)
        //{
        //    indexResult[itemIndex[i]] = i;
        //}

        int idx = 0; // 배열 인덱스

        while (count > 0)
        {
            int getResultKey = curprobability.OrderBy(x => (summonResultValue[idx] - x >= 0)).First(); // 나중에 이진 탐색으로 줄여봅시다
            curLevelTable.TryGetValue(getResultKey, out string index);
            //Debug.Log($"idx : {idx}, summonResultValue : {summonResultValue[idx]}, getResultKey : {getResultKey}, index : {index}");
            resultIdList.Add(index);
            // 확인용 획득 수 카운트 증가
            //indexResult.TryGetValue(index, out int result);
            //testResult[result]++;
            count--;
            idx++;
            if (summonTable.ApplySummonCount())
            {
                // 이거 trygetvalue 다시 안해도 될듯 나중에 확인
                SummonTables.TryGetValue(typeLink, out var newSummonTable);
                curLevelTable = newSummonTable.GetProbabilityTable();
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
        _shopSummon.BannerUpdate(typeLink);
        summonTable.ApplySummonCountAdd();
        summonResurt.Clear();
        resultIdList.Clear();

        // Data Save
        Manager.Data.Save();
    }

    private void EquipmentAdd(string[] summonResult)
    {
        for (int i = 0; i < summonResult.Length; i++)
        {
            UserItemData itemData = _inventoryManager.SearchItem(summonResult[i]);
            itemData.hasCount++;
        }
    }

    #endregion

    #region Debug Method

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


