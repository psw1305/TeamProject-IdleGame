using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestManager
{
    #region Fields

    // json 저장
    private string questjsonPath = Application.dataPath + "/Scripts/Json/QuestDBTest.json";
    private string questjsonText;

    
    // Quest DB
    public QuestData CurrentQuest;
    public QuestDataBase QuestDataBase;

    public int questNum;
    public int QuestIndex;
    public QuestData[] QuestDB = new QuestData[4] ;

    public QuestDamageUp DamageUp = new QuestDamageUp();
    public QuestHPUp HPUp = new QuestHPUp();
    public QuestDefeatEnemy DefeatEnemy = new QuestDefeatEnemy();
    public QuestReachStage ReachStage = new QuestReachStage();

    #endregion

    public void SetDataPath(string jsonPath)
    {
        this.questjsonPath = Application.dataPath + jsonPath;
    }

    #region Init

    public void InitQuest()
    {
        LoadQuestdataBase();
    }

    #endregion

    #region Save Load Json

    public void SaveQuestDataBase()
    {
        string questJson = JsonUtility.ToJson(QuestDataBase, true);
        File.WriteAllText(questjsonPath, questJson);
        Debug.Log("퀘스트 데이터 베이스 저장 완료");
    }

    public void LoadQuestdataBase()
    {
        questjsonText = File.ReadAllText(questjsonPath);
        QuestDataBase = JsonUtility.FromJson<QuestDataBase>(questjsonText);
        Debug.Log("퀘스트 데이터 베이스 불러오기 완료");

        DamageUp.Init(QuestDataBase.QuestNum, QuestDB.Length);
        HPUp.Init(QuestDataBase.QuestNum, QuestDB.Length);
        DefeatEnemy.Init(QuestDataBase.QuestNum, QuestDB.Length);
        ReachStage.Init(QuestDataBase.QuestNum, QuestDB.Length);

        QuestDB[0] = DamageUp;
        QuestDB[1] = HPUp;
        QuestDB[2] = DefeatEnemy;
        QuestDB[3] = ReachStage;

        questNum = QuestDataBase.QuestNum;
        QuestIndex = QuestDataBase.QuestNum % QuestDB.Length;
        CurrentQuest = QuestDB[QuestIndex];

        Debug.Log($"CurrentQuest : {CurrentQuest.questObjective}, index : {QuestIndex}");
    }

    #endregion


    // 퀘스트 클리어 여부 확인
    public void CheckQuestCompletion()
    {
        // 목표 Value < 현재 Value 일 때
        if (CurrentQuest.objectiveValue <= CurrentQuest.currentValue)
        {
            // isClear는 필요한 변수인가? UI상으로 표시할때 사용하는 것인가? 
            CurrentQuest.isClear = true;
            EarnQuestReward();
            NextQuest();
        }
        else
        {
            CurrentQuest.isClear = false;
        }
    }

    // 다음 퀘스트로 넘어가기
    public void NextQuest()
    {
        CurrentQuest.ObjectiveValueUp();
        CurrentQuest.isClear = false;
        QuestDataBase.QuestNum++;
        QuestIndex++;

        if (QuestIndex >= QuestDB.Length)
        {
            QuestIndex = 0;
            QuestDB[2].currentValue = 0; // 몬스터 사냥 횟수 초기화
        }

        CurrentQuest = QuestDB[QuestIndex];

        SaveQuestDataBase();
    }

    // 퀘스트 값 증가 
    public void QuestCurrentValueUp()
    {
        CurrentQuest.currentValue++;
        SaveQuestDataBase();
    }

    public void EarnQuestReward()
    {
        Manager.Game.Player.RewardGem(500);
    }

    //public void QuestObjectiveValueUp()
    //{
    //    // TODO: 일단 올라가야 되는 값들만 올라갔습니다.index접근이 아닌, QuestType별로 들어가야합니다.
    //    QuestDB[0].objectiveValue += QuestDataBase.QuestNum * ValueUpRate;
    //    QuestDB[1].objectiveValue += QuestDataBase.QuestNum * ValueUpRate;
    //    QuestDB[3].objectiveValue *= 2;
    //}
}

# region Quest DataBase

[System.Serializable]
public class QuestDataBase
{
    public int QuestNum;
}

[System.Serializable]
public class QuestData
{
    public QuestType questType;
    public string questObjective;
    public int ValueUpRate;
    public int objectiveValue;
    public int currentValue;
    public bool isClear;

    // 추상클래스에서 변경...
    public virtual void Init(int questLevel, int questCount) { }

    public void ObjectiveValueUp()
    {
        objectiveValue *= ValueUpRate;
    }
}
#endregion