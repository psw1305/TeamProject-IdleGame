using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class NPCManager : BehaviourSingleton<NPCManager>
{
    [SerializeField] private GameObject _pivot;
    [SerializeField] private TextMeshProUGUI _comment;
    [SerializeField] private UINPCPortrait npcPortrait;

    private string[] _currentScript;
    private Dictionary<string, string[]> _scriptsIndex = new Dictionary<string, string[]>();

    private int index;

    public bool isFirstShop;
    public bool isFirstPlay;
    public bool isFirstRanking;

    protected override void Awake()
    {
        base.Awake();

        // 인스펙터 상에서 IsFirst를 체크하면 NPC가 나오도록 테스트가 할 수 있습니다.
        if (isFirstShop)
            PlayerPrefs.SetInt("ShopTutorial", 0);

        if (isFirstPlay)
            PlayerPrefs.SetInt("StartTutorial", 0);
        
        if (isFirstRanking)
            PlayerPrefs.SetInt("RankingTutiroal", 0);


        if (!PlayerPrefs.HasKey("StartTutorial") && PlayerPrefs.GetInt("StartTutorial") == 0)
            PlayerPrefs.SetInt("StartTutorial", 0);


        if (!PlayerPrefs.HasKey("ShopTutorial") && PlayerPrefs.GetInt("ShopTutorial") == 0)
            PlayerPrefs.SetInt("ShopTutorial", 0);

        if (!PlayerPrefs.HasKey("RankingTutiroal") && PlayerPrefs.GetInt("RankingTutiroal") == 0)
            PlayerPrefs.SetInt("RankingTutiroal", 0);
    }

    private void Start()
    {
        _scriptsIndex.Add("StartTutorial", new string[] { "안녕하세요 용사님! 처음 뵙겠습니다:0:0:0",
                                                      "저는 안내를 맡은 접수원 에리나 입니다:0:2:0",
                                                      "이제 전투를 하러 가야하는데요:0:1:0",
                                                      "몬스터와 보스를 사냥한 <color=yellow>골드</color>로 능력치를 강화하고:0:0:0",
                                                      "<color=red>스킬</color>과 <color=red>동료</color>를 <color=red>수집</color>해 더욱 강해지세요!:0:5:0"});

        _scriptsIndex.Add("ShopTutorial", new string[] { "이곳은 장비와 동료, 스킬을 구매할 수 있는 공간입니다:1:3:2",
                                                    "다이아로 구매 하거나, 무료로 받을 수도 있습니다:1:0:0",
                                                    "무료 상품은 받을 때마다 1개씩 더 받을 수 있어요:1:2:1",
                                                    "최대 <color=red>35</color>개 까지 늘어습니다:0:3:0",
                                                    "많이 구매 할수록 상품 레벨이 높아집니다:0:5:0",
                                                    "등급이 높아질 수록 좋은 아이템을 얻을 수 있답니다:0:0:0",
                                                    "그럼 즐거운 시간 되세요!:0:2:0"});

        _scriptsIndex.Add("RankingTutiroal", new string[] {"이곳에서는 우리 사무소의 순위를 알 수 있어요:0:0:0",
                                                           "얼마나 많은 보스를 잡고 나아갔는지에 따라 점수를 얻습니다:0:0:5", 
                                                           "다른 사무소들보다 더 높은 점수를 획득해 봅시다!:0:0:2"});
    }
        
    public void UnactiveNPC()
    {
        _pivot.SetActive(false);
    }

    public void ActiveNPC(string scritsIndex)
    {
        _pivot.SetActive(true);
        _currentScript = _scriptsIndex[scritsIndex];
        index = 0;
        _comment.text = _currentScript[index].Split(":")[0];
        CallSpriteChange();
    }

    public void TalkAction()
    {
        index++;
        //index 초과하면 대화 종료
        if (index >= _currentScript.Length)
        {
            UnactiveNPC();
            return;
        }

        CallSpriteChange();
        _comment.text = _currentScript[index].Split(":")[0];
    }


    private void CallSpriteChange()
    {
        npcPortrait.SpriteChange(int.Parse(_currentScript[index].Split(":")[1]),
                                 int.Parse(_currentScript[index].Split(":")[2]),
                                 int.Parse(_currentScript[index].Split(":")[3]));
    }
}