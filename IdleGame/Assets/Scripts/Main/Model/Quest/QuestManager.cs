using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestManager
{
    #region Fields

    //private IQuestStractegy questStrategy;

    public Quest Quest;

    // json 저장
    private string QuestjsonPath = Application.dataPath + "/Scripts/Json/QuestDBTest.json";
    private string QuestjsonText;

    // Quest DB
    public QuestData CurrentQuest;
    public QuestDataBase QuestDataBase;

    private int ValueUpRate = 5; // 퀘스트 사이클 올라갈 때마다 곱해지는 값

    #endregion

    #region constructor 

    public QuestManager() { }

    //public QuestManager(IQuestStractegy strategy)
    //{
    //    this.questStrategy = strategy;
    //}

    //public void SetQuestStrategy(IQuestStractegy strategy)
    //{

    //}

    public void Achieve()
    {
        //this.questStrategy.QuestAchive();
    }

    #endregion 

    #region Init

    public void InitQuest()
    {
        LoadQuestdataBase();
        Quest.Init();
    }

    #endregion

    #region Save Load Json

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
        CurrentQuest = QuestDataBase.QuestDB[QuestDataBase.QuestIndex];

        // Player의 업그레이드 값 가져오기 
        QuestDataBase.QuestDB[0].currentValue = Manager.Game.Player.AttackDamage.Level;
        QuestDataBase.QuestDB[1].currentValue = Manager.Game.Player.Hp.Level;
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
        QuestDataBase.QuestNum++;
        QuestDataBase.QuestIndex++;

        if (QuestDataBase.QuestIndex >= QuestDataBase.QuestDB.Count)
        {
            QuestDataBase.QuestIndex = 0;

            QuestObjectiveValueUp();
            foreach (var quest in QuestDataBase.QuestDB)
            {
                quest.isClear = false;
            }
            QuestDataBase.QuestDB[2].currentValue = 0; // 몬스터 사냥 횟수 초기화
        }

        CurrentQuest = QuestDataBase.QuestDB[QuestDataBase.QuestIndex];

        SaveQuestDataBase();
    }

    // 퀘스트 값 증가 
    public void QuestCurrentValueUp()
    {
        CurrentQuest.currentValue++;

        // 값이 올라갈 때 마다 체크하는 것이 아니라, 퀘스트를 클릭할 때 완료됐는지 체크
        // 하지만 UI는 클릭하지 않아도 완료되었다는 이펙트가 표시되는데 매번 체크하는 것인가?
        // CheckQuestCompletion();

        // 값이 올라갈 때 마다 Save를 해야하는가?
        SaveQuestDataBase();
    }

    public void EarnQuestReward()
    {
        Manager.Game.Player.RewardGem(500);
    }

    public void QuestObjectiveValueUp()
    {
        // TODO: 일단 올라가야 되는 값들만 올라갔습니다.index접근이 아닌, QuestType별로 들어가야합니다.
        QuestDataBase.QuestDB[0].objectiveValue += QuestDataBase.QuestNum * ValueUpRate;
        QuestDataBase.QuestDB[1].objectiveValue += QuestDataBase.QuestNum * ValueUpRate;
        QuestDataBase.QuestDB[3].objectiveValue *= 2;
    }
}

# region Quest DataBase

[System.Serializable]
public class QuestDataBase
{
    public int QuestNum;
    public int QuestIndex;
    public List<QuestData> QuestDB;
}

[System.Serializable]
public class QuestData
{
    public QuestType questType;
    public string questObjective;
    public int objectiveValue;
    public int currentValue;
    public bool isClear;
}

#endregion