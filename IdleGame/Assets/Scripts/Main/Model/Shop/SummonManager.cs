using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SummonManager
{
    #region Fields

    private string _jsonPath = $"{Application.dataPath}/Resources/Data/SummonTable/EquipmentSummonTable.json";
    //private string _jsonPath = Application.dataPath + "/Resources/Data/SummonTable/EquipmentSummonTable.json";
    private string _tableText;
    private Player _player;
    private InventoryManager _inventoryManager;

    private int[] probability;
    private int[] itemIndex;
    private ProbabilityDataTable _probabilityDataTable;
    private Dictionary<int, int> probabilityTable = new Dictionary<int, int>();
    private List<int> summonResurt = new List<int>(1000);

    // 확인용
    private int[] testResult;
    private Dictionary<int, int> indexResult = new Dictionary<int, int>();

    #endregion

    #region Initialize

    public void Initialize()
    {
        ProbabilityInit();
        _player = Manager.Game.Player;
        _inventoryManager = Manager.Inventory;
    }

    private void ProbabilityInit()
    {
        _tableText = File.ReadAllText(_jsonPath);
        _probabilityDataTable = JsonUtility.FromJson<ProbabilityDataTable>(_tableText);

        probability = _probabilityDataTable.Probabilities.Select(x => x.Probability).ToArray();
        itemIndex = _probabilityDataTable.Probabilities.Select(x => x.ItemId).ToArray();

        int sum = 0;

        // 확률 누계 딕셔너리 만들기
        for (int i = 0; i < probability.Length; i++)
        {
            sum += probability[i];
            probabilityTable.Add(sum, itemIndex[i]);
            // 확인용 배열 연결
            indexResult.Add(itemIndex[i], i);
        }

        testResult = new int[probabilityTable.Count];
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

[System.Serializable]
public class ProbabilityDataTable
{
    public List<ProbabilityData> Probabilities;
}

[System.Serializable]
public class ProbabilityData
{
    public int ItemId;
    public int Probability;
}
