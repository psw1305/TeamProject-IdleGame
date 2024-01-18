using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class QuestManager
{
    private string QuestjsonPath = Application.dataPath + "/Scripts/Json/QuestDBTest.json";
    private string QuestjsonText;

    private QuestData _cuurntQuest;

    public QuestDataBase questDataBase;

    public void SaveQuestDataBase()
    {
        string questJson = JsonUtility.ToJson(questDataBase, true);
        File.WriteAllText(QuestjsonPath, questJson);
        Debug.Log("퀘스트 데이터 베이스 저장 완료");
    }

    public void LoadQuestdataBase()
    {
        QuestjsonText = File.ReadAllText(QuestjsonPath);
        questDataBase = JsonUtility.FromJson<QuestDataBase>(QuestjsonText);
        Debug.Log("퀘스트 데이터 베이스 불러오기 완료");

        _cuurntQuest = questDataBase.QuestDB[1];
    }


    // 퀘스트 클리어 여부 확인
    public void CheckQuestCompletion()
    {
        // 임시. 목표 Value < 현재 Value 일 때
        if(questDataBase.QuestDB[questDataBase.QuestIndex].objectValue < questDataBase.QuestDB[questDataBase.QuestIndex].currentValue)
        {
            questDataBase.QuestDB[questDataBase.QuestIndex].isClear = true;
        }
        else
        {
            questDataBase.QuestDB[questDataBase.QuestIndex].isClear = false;
        }
    }

    // 다음 퀘스트로 넘어가기
    public void NextQuest()
    {
        questDataBase.QuestIndex++;

        if (questDataBase.QuestIndex >= questDataBase.QuestDB.Count)
        {
            questDataBase.QuestIndex = 0;
        }
    }

    // 퀘스트 값 증가 
    public void QuestObjectiveValueUp()
    {
        _cuurntQuest.objectValue++;
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