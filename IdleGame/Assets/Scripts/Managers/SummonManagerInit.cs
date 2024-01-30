using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SummonManager
{
    private Dictionary<string, SummonTable> _table = new Dictionary<string, SummonTable>();
    private SummonBlueprint _summonBlueprint;
    public Dictionary<string, SummonTable> SummonTable => _table;

    public void TableInitalize(string typeLink)
    {
        SummonTable table = new SummonTable(typeLink);
        _table[typeLink] = table;
    }
}

public class SummonTable
{
    #region Fields

    private Dictionary<int, Dictionary<int, string>> probabilityTable = new();
    private List<int> _gradeUpCount;

    #endregion

    #region Properties

    public int SummonGrade { get; private set; }
    public int SummonCounts { get; private set; }
    public int GetCurCount => SummonCounts - _gradeUpCount[SummonGrade - 1];
    public int GetNextCount => _gradeUpCount[SummonGrade] - _gradeUpCount[SummonGrade - 1];

    #endregion

    public SummonTable(string tableLink)
    {
        ProbabilityInit(tableLink);
        GradeCountInit(tableLink);
        SummonGradeInit();
    }

    #region Initialize

    private void ProbabilityInit(string tableLink)
    {
        string _tabletext = Manager.Resource.GetFileText($"SummonTable{tableLink}");
        var probabilityDataTable = JsonUtility.FromJson<ProbabilityDataTable>($"{{\"probabilityDataTable\":{_tabletext}}}");

        // 불러온 테이블을 레벨 그룹별로 1차 가공
        // <등급(그룹), <아이템, 확률>>
        var gradeValue = probabilityDataTable.probabilityDataTable
            .GroupBy(data => data.SummonGrade)
            .ToDictionary(grade => grade.Key, group => group.ToDictionary(x => x.ItemId, x => x.Probability));

        // 1차 가공된 그룹을 <확률 누적, 아이템> 그룹으로 2차 가공
        // <등급(그룹), <확률 누계, 아이템>>
        probabilityTable = gradeValue
            .ToDictionary(gradeGroup => gradeGroup.Key, gradeGroup =>
            {
                var cumulativeDict = new Dictionary<int, string>();
                int sum = 0;

                // 들어온 gradeGroup은 딕셔너리므로 foreach를 쓰는것이 좋다
                foreach (var probData in gradeGroup.Value)
                {
                    sum += probData.Value; // 확률 누적
                    cumulativeDict[sum] = probData.Key; // 확률 누적값을 키로, 아이템 ID를 값으로 설정
                }

                return cumulativeDict;
            }
            );

        // 확률 확인
        //DebugTableData();
    }

    private void GradeCountInit(string tableLink)
    {
        string _tabletext = Manager.Resource.GetFileText($"SummonCount{tableLink}");
        var gradeUpDataTable = JsonUtility.FromJson<GradeUpDataTable>($"{{\"gradeUpDataTable\":{_tabletext}}}");

        _gradeUpCount = gradeUpDataTable.gradeUpDataTable.Select(x => x.needCounts).ToList();
        _gradeUpCount.Add(-1);
    }
    
    private void SummonGradeInit()
    {
        //TODO = 플레이어의 소환 타입별 카운트 불러오기
        SummonCounts = 0;

        int CurCount = _gradeUpCount.OrderBy(x => (SummonCounts - x >= 0)).First();

        for (int i = 0; i < _gradeUpCount.Count; i++)
        {
            if (_gradeUpCount[i] == CurCount)
            {
                SummonGrade = i;
            }
        }
    }

    #endregion


    #region Control Method

    public Dictionary<int, string> GetProbabilityTable()
    {
        probabilityTable.TryGetValue(SummonGrade, out var summonProbability);
        return summonProbability;
    }

    /// <summary>
    /// 소환 횟수(SummonCount)를 늘리면서 소환 등급도 체크합니다.
    /// </summary>
    public bool ApplySummonCount()
    {
        SummonCounts++;
        if (SummonCounts >= _gradeUpCount[SummonGrade] && _gradeUpCount[SummonGrade] > -1)
        {
            SummonGrade++;
            return true;
        }
        return false;
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

    #endregion
}

#region Table Serializable Class

[System.Serializable]
public class ProbabilityDataTable
{
    public List<ProbabilityData> probabilityDataTable;
}

[System.Serializable]
public class ProbabilityData
{
    public int SummonGrade;
    public string ItemId;
    public int Probability;
}

[System.Serializable]
public class GradeUpDataTable
{
    public List<GradeUpData> gradeUpDataTable;
}

[System.Serializable]
public class GradeUpData
{
    public int SummonGrade;
    public int needCounts;
}

#endregion