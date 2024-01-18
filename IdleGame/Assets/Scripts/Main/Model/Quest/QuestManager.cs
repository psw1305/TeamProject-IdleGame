using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class QuestManager
{
    private string QuestjsonPath = Application.dataPath + "/Scripts/Json/QuestDBTest.json";
    private string QuestjsonText;

    public QuestData CuurntQuest;

    public QuestDataBase QuestDataBase;
    
    public void SaveQuestDataBase()
    {
        string questJson = JsonUtility.ToJson(QuestDataBase, true);
        File.WriteAllText(QuestjsonPath, questJson);
        Debug.Log("퀘스트 데이터 베이스 저장 완료");
    }

    public void LoadQuestdataBase()
    {
        QuestjsonText = File.ReadAllText(QuestjsonPath);
        QuestDataBase = JsonUtility.FromJson<QuestDataBase>(QuestjsonText);
        Debug.Log("퀘스트 데이터 베이스 불러오기 완료");

        //현재 퀘스트를 Index에 저장한 값으로 불러오기
        CuurntQuest = QuestDataBase.QuestDB[QuestDataBase.QuestIndex];
    }

    public void InitQuest()
    {
        LoadQuestdataBase();
    }

    // 퀘스트 클리어 여부 확인
    public void CheckQuestCompletion()
    {
        // 목표 Value < 현재 Value 일 때
        if(CuurntQuest.objectValue <= CuurntQuest.currentValue)
        {
            // isClear는 필요한 변수인가? UI상으로 표시할때 사용하는 것인가? 
            CuurntQuest.isClear = true;            
            NextQuest();
        }
        else
        {
            CuurntQuest.isClear = false;
        }
    }

    // 다음 퀘스트로 넘어가기
    public void NextQuest()
    {
        QuestDataBase.QuestIndex++;

        if (QuestDataBase.QuestIndex >= QuestDataBase.QuestDB.Count)
        {
            QuestDataBase.QuestIndex = 0;
            foreach(var quest in QuestDataBase.QuestDB)
            {
                quest.isClear = false;
                quest.currentValue = 0;
            }
        }

        CuurntQuest = QuestDataBase.QuestDB[QuestDataBase.QuestIndex];

        SaveQuestDataBase();
    }

    // 퀘스트 값 증가 
    public void QuestObjectiveValueUp()
    {
        CuurntQuest.currentValue++;

        // 값이 올라갈 때 마다 체크하는 것이 아니라, 퀘스트를 클릭할 때 완료됐는지 체크
        // 하지만 UI는 클릭하지 않아도 완료되었다는 이펙트가 표시되는데 매번 체크하는 것인가?
        // CheckQuestCompletion();

        // 값이 올라갈 때 마다 Save를 해야하는가?
        Debug.Log($"퀘스트 현재 값: {CuurntQuest.currentValue}");
        SaveQuestDataBase();
    }
}

[System.Serializable]
public class QuestDataBase
{
    public int QuestIndex;
    public List<QuestData> QuestDB;
}

[System.Serializable]
public class QuestData
{
    public QuestType questType;
    public string questObjective;
    public int objectValue;
    public int currentValue;
    public bool isClear;
}