using UnityEngine;

public class QuestManager
{
    #region Fields

    private int questIndex;

    // Quest DB
    private QuestDamageUp DamageUp = new();
    private QuestHPUp HPUp = new();
    private QuestDefeatEnemy DefeatEnemy = new();
    private QuestReachStage ReachStage = new();

    private GameObject questClearEffect;

    #endregion

    #region Properties

    public int QuestNum { get; private set; }
    public int DefeatQuestValue { get; private set; }
    public QuestData[] QuestDB { get; private set; }
    public QuestData CurrentQuest { get; private set; }

    #endregion

    #region Init

    public void InitQuest()
    {
        // 데이터 불러오기
        QuestNum = Manager.Data.Profile.Quest_Complete;
        QuestDB = new QuestData[4];

        LoadQuestdataBase();
        questClearEffect = GameObject.Find("Yellow blur");
        ClearEffectOnOff();
    }

    #endregion

    #region Save Load Json

    public void LoadQuestdataBase()
    {
        DamageUp.Init(QuestNum, QuestDB.Length);
        HPUp.Init(QuestNum, QuestDB.Length);
        DefeatEnemy.Init(QuestNum, QuestDB.Length);
        ReachStage.Init(QuestNum, QuestDB.Length);

        QuestDB[0] = DamageUp;
        QuestDB[1] = HPUp;
        QuestDB[2] = DefeatEnemy;
        QuestDB[3] = ReachStage;

        questIndex = QuestNum % QuestDB.Length;
        CurrentQuest = QuestDB[questIndex];
    }

    #endregion

    public bool IsQuestComplete()
    {
        if (CurrentQuest.objectiveValue <= CurrentQuest.currentValue)
            CurrentQuest.isClear = true;
        else
            CurrentQuest.isClear = false;

        return CurrentQuest.isClear;
    }

    public void ClearEffectOnOff()
    {
        if (IsQuestComplete())
            questClearEffect.SetActive(true);
        else
            questClearEffect.SetActive(false);
    }

    // 다음 퀘스트로 넘어가기
    public void NextQuest()
    {
        CurrentQuest.ObjectiveValueUp();
        CurrentQuest.isClear = false;
        
        // 사냥 퀘스트 일 경우, 진행 값 초기화
        if (CurrentQuest.questType == QuestType.DefeatEnemy)
        {
            QuestDB[2].currentValue = 0;
            DefeatQuestValue = 0;
        }

        QuestNum++;
        questIndex++;

        if (questIndex >= QuestDB.Length) questIndex = 0;

        CurrentQuest = QuestDB[questIndex];
        ClearEffectOnOff();

        // 데이터 저장
        Manager.Data.Save();
    }

    public void QuestCurrentValueUp()
    {
        CurrentQuest.currentValue++;       
        ClearEffectOnOff();

        // 사냥 진행 데이터 저장
        if (CurrentQuest.questType == QuestType.DefeatEnemy)
        {
            DefeatQuestValue = CurrentQuest.currentValue;
        }
    }

    public void EarnQuestReward()
    {
        Manager.Game.Player.RewardGem(500);
    }
}

# region Quest Data

public abstract class QuestData
{
    public QuestType questType;
    public string questObjective;
    public int ValueUpRate;
    public int objectiveValue;
    public int currentValue;
    public bool isClear;

    public abstract void Init(int questLevel, int questCount);

    public abstract void ObjectiveValueUp();
}
#endregion