using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;

public class UIPopupShopSummon : UIPopup
{
    #region Fields

    private Player _player;

    private Button _checkBtn;
    private Button _summonTryBtn_11;
    private Button _summonTryBtn_35;
    private Button _closeBtn;

    private int[] probability = new int[] { 5000, 2500, 1500, 700, 250, 50 };
    private int[] index;
    private Dictionary<int, int> probabilityTable = new Dictionary<int, int>();
    private List<int> summonResurt = new List<int>(1000);
    private int[] testResurt;

    #endregion

    #region Initialize

    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        SetButtonEvents();
        ProbabilityInit();

        _player = Manager.Game.Player;
    }


    private void SetButtonEvents()
    {
        SetUI<Button>();
        _checkBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
        _summonTryBtn_11 = SetButtonEvent("Btn_Summon_35Repeat", UIEventType.Click, SummonTry);
        _summonTryBtn_35 = SetButtonEvent("Btn_Summon_35Repeat", UIEventType.Click, SummonTry);
        _closeBtn = SetButtonEvent("CloseButton", UIEventType.Click, CloseSummonPopup);
    }

    #endregion


    #region Button Events

    private void CloseSummonPopup(PointerEventData eventData)
    {
        // TODO : 방치 보상 확인 시 보상 획득 추가
        Manager.UI.ClosePopup();
    }
    #endregion

    #region Summon Test

    private void ProbabilityInit()
    {
        int sum = 0;
        index = new int[probability.Length];
        testResurt = new int[probability.Length];

        for (int i = 0; i < probability.Length; i++)
        {
            index[i] = i;
        }

        for (int i = 0; i < probability.Length; i++)
        {
            sum += probability[i];
            probabilityTable.Add(sum, index[i]);
        }
    }

    private void SummonTry(PointerEventData eventData)
    {
        if (_player.Gems < 0)
        {
            Debug.Log("Not enough Gems");
            return;
        }
        Summon();
    }

    private void Summon()
    {
        for (int i = 0; i < 35; i++)
        {
            summonResurt.Add(Random.Range(0, 10000));
        }
        int[] summonResultValue = summonResurt.ToArray();
        summonResurt.Clear();

        int[] summonValueKey = probabilityTable.Select(x => x.Key).ToArray();

        for (int i = 0; i < summonResultValue.Length; i++)
        {
            int getResultKey = summonValueKey.OrderBy(x => (summonResultValue[i] - x > 0)).First(); // 나중에 이진 탐색으로 줄여봅시다
            probabilityTable.TryGetValue(getResultKey, out int index);
            summonResurt.Add(index);
            testResurt[index]++;
        }
        Debug.Log($"0 : {testResurt[0]}, 1 : {testResurt[1]}, 2 : {testResurt[2]}, 3 : {testResurt[3]}, 4 : {testResurt[4]}, 5 : {testResurt[5]}");
        int[] finalResult = summonResurt.ToArray();
        summonResurt.Clear();
        for (int i = 0; i < testResurt.Length; i++)
        {
            testResurt[i] = 0;
        }
    }


    #endregion
}
