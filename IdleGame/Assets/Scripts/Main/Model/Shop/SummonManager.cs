using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SummonManager
{
    #region Fields

    private string _jsonPath = $"{Application.dataPath}/Resources/Data/SummonTable/EquipmentSummonTable.json";
    private string _tableText;
    private Player _player;
    private InventoryManager _inventoryManager;

    private int[] probability;
    private int[] itemIndex;
    private ProbabilityDataTable _probabilityDataTable;
    private Dictionary<int, Dictionary<int, int>> probabilityTable = new();
    private List<int> summonResurt = new List<int>(1000);

    // 확인용
    private int[] testResult;
    private Dictionary<int, int> indexResult = new();

    #endregion

    #region Properties

    public int SummonLevel { get; private set; }
    public int SummonCounts { get; private set; }

    #endregion

    #region Initialize

    public void SetSummon()
    {
        _player = Manager.Game.Player;
        _inventoryManager = Manager.Inventory;
    }

    public void Initialize()
    {
        SummonLevel = 2;

        ProbabilityInit();
    }

    private void ProbabilityInit()
    {
        _tableText = File.ReadAllText(_jsonPath);
        _probabilityDataTable = JsonUtility.FromJson<ProbabilityDataTable>($"{{\"probabilityDataTable\":{_tableText}}}");

        // 불러온 테이블을 레벨 그룹별로 1차 가공
        var gradeValue = _probabilityDataTable.probabilityDataTable
            .GroupBy(data => data.SummonGrade)
            .ToDictionary(grade => grade.Key, group => group.ToDictionary(x => x.Probability, x => x.ItemId));
        
        // 1차 가공된 그룹을 <확률 누적, 아이템> 그룹으로 2차 가공
        probabilityTable = gradeValue
            .ToDictionary(gradeGroup => gradeGroup.Key, gradeGroup =>
                {
                    var cumulativeDict = new Dictionary<int, int>();
                    int sum = 0;

                    // 들어온 gradeGroup은 딕셔너리므로 foreach를 쓰는것이 좋다
                    foreach (var probData in gradeGroup.Value)
                    {
                        sum += probData.Key; // 확률 누적
                        cumulativeDict[sum] = probData.Value; // 확률 누적값을 키로, 아이템 ID를 값으로 설정
                    }

                    return cumulativeDict;
                }
            );
    }

    #endregion

        #region Summon Test

    public void SummonTry(int price, int count)
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

        // 소환 레벨에서 딕셔너리 키(누적 확률)만 뽑은 후 랜덤값보다 높은 숫자 중 가장 가까운 키를 찾아 인덱스 반환
        probabilityTable.TryGetValue(SummonLevel, out var summonProbability);
        var probability = summonProbability.Select(x => x.Key).ToArray();

        // 테스트 결과 확인용 배열 세팅
        testResult = new int[summonProbability.Count];
        itemIndex = summonProbability.Select(x => x.Value).ToArray();
        for (int i = 0; i < itemIndex.Length; i++)
        {
            indexResult[itemIndex[i]] = i;
        }

        for (int i = 0; i < summonResultValue.Length; i++)
        {
            int getResultKey = probability.OrderBy(x => (summonResultValue[i] - x > 0)).First(); // 나중에 이진 탐색으로 줄여봅시다
            summonProbability.TryGetValue(getResultKey, out int index);
            summonResurt.Add(index);
            // 확인용 획득 수 카운트 증가
            indexResult.TryGetValue(index, out int result);
            testResult[result]++;
        }
        TestDebugLog();

        int[] finalResult = summonResurt.ToArray();
        EquipmentAdd(finalResult);
        var popup = Manager.UI.ShowPopup<UIPopupDynamicRewardPopup>("DynamicRewardPopup");
        //popup.DataInit(finalResult);
        //popup.PlayStart();
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

[System.Serializable]
public class ProbabilityDataTable
{
    public List<ProbabilityData> probabilityDataTable;
}

[System.Serializable]
public class ProbabilityData
{
    public int SummonGrade;
    public int ItemId;
    public int Probability;
}
