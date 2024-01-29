using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class SummonManager
{
    #region Fields

    private List<Dictionary<int, Dictionary<int, string>>> tableList = new();
    private Dictionary<int, Dictionary<int, string>> probabilityTable = new();
    private List<int> summonResurt = new List<int>(500);
    private List<string> resultIdList = new List<string>(500);

    #endregion

    #region Initialize

    private void ProbabilityInit()
    {
        _tableText = Manager.Resource.GetFileText("SummonTableEquipment");
        var probabilityDataTable = JsonUtility.FromJson<ProbabilityDataTable>($"{{\"probabilityDataTable\":{_tableText}}}");

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

    private void SummonLevelInitialize()
    {

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

#endregion