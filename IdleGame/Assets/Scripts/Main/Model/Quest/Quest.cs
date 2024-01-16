using Unity.VisualScripting;
using UnityEngine;

public class Quest : MonoBehaviour
{
    private QuestBlueprint _questBlueprint;

    private QuestType _questType;
    private int _questNum;              // 퀘스트 넘버
    private int _Questreward = 500;   // 잼 보상 500 고정

    private int _value;
    private int _valueRate;
    private bool _isClear;

    private int Questindex;
    private int QuestMaxLength = System.Enum.GetValues(typeof(QuestType)).Length;


    public void Initialize(QuestBlueprint questBlueprint)
    {
        _questBlueprint = questBlueprint;
    }

    public void QuestCycle()
    {
        if(Questindex >= QuestMaxLength)
        {
            _questType++;   
            Questindex = 0;
        }
    }

    public void IsQuestClear()
    {
        //퀘스트 타입마다 각자 존재하는 숫자가 필요하다. 
        if (_value > _questBlueprint.ObjectiveValue)
            _isClear = true;
        else
            _isClear = false;
    }


    // 퀘스트 수치 저장 메서드
    // 값을 불러오면 다시 퀘스트 라인이 유지되도록 값이 들어간다.


}
