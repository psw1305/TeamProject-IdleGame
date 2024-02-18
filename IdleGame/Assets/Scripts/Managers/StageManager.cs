using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager
{
    #region Fields

    // json 경로 관련
    private string _tableText;
    private string _UItableText;

    private Dictionary<int, StageData> stageTable;
    private Dictionary<int, StageUIData> stageUITable;
    private StageData stageData;
    private List<BaseEnemy> enemyList = new();
    private Transform[] spawnPoint;
    private Transform bossSpawnPoint;
    private Coroutine stageCoroutine;
    private UISceneMain uISceneMain;

    private BackgroundControl backgroundControl;

    // 스케일 비율 
    private float ratio;
    #endregion

    #region Properties

    // 플레이 데이터 프로퍼티
    public int Chapter { get; private set; }
    public int StageLevel { get; private set; }
    public bool WaveLoop { get; private set; }

    // 관리용 프로퍼티
    public bool BossAppearance => StageLevel == StageConfig.BattleCount;
    public bool StageClear => StageLevel > StageConfig.BattleCount;
    public bool WaveClear => enemyList.Count == 0;
    public bool PlayerReset { get; private set; }

    // 스테이지 정보 로드용 프로퍼티
    public StageBlueprint StageConfig => Manager.Address.GetBlueprint(stageData.StageConfig) as StageBlueprint;
    public string StageBackground => string.Empty;
    public int EnemyStatRate => stageData.EnemyStatRate;
    public int EnemyGoldRate => stageData.EnemyGoldRate;
    public int EnemySpawnCount => stageData.EnemySpawnCount;
    public int IdleGoldReward => stageData.IdleGoldReward * EnemyGoldRate;

    #endregion

    #region Table Reference

    /// <summary>
    /// 씬 -> 세션 생성 -> 게임 시작 순서. 여기서 스테이지 정보 및 UI 갱신
    /// </summary>
    /// <param name="index"></param>
    private void StageDataChange(int index)
    {
        stageTable.TryGetValue(index, out var data);
        stageData = data;
    }

    #endregion

    #region Init

    public void Initialize()
    {
        // json 파일 로딩, 딕셔너리에 인덱스 그룹 넣기
        _tableText = Manager.Address.GetText("ItemTableStage");
        var stageDataTable = JsonUtility.FromJson<StageDataTable>($"{{\"stageDataTable\":{_tableText}}}");

        _UItableText = Manager.Address.GetText("ItemTableStage_Hud");
        var stageUIDataTable = JsonUtility.FromJson<StageUIDataTable>($"{{\"stageUIDataTable\":{_UItableText}}}");

        stageTable = stageDataTable.stageDataTable.ToDictionary(group => group.Index, group => group);
        stageUITable = stageUIDataTable.stageUIDataTable.ToDictionary(group => group.Index, group => group);

        var profile = Manager.Data.Profile;
        Chapter = profile.Stage_Chapter;
        StageLevel = profile.Stage_Level;
        WaveLoop = profile.Stage_WaveLoop;

        ratio = Manager.Game.screenRatio;

        backgroundControl = Object.FindObjectOfType<BackgroundControl>();
        backgroundControl.Initiailize();
    }

    public void SetStage(Transform[] spawnPoint, Transform bossSpawnPoint)
    {
        StageDataChange(Chapter);
        Manager.Game.Player.IdleRewardInit();

        this.spawnPoint = spawnPoint;
        this.bossSpawnPoint = bossSpawnPoint;

        uISceneMain = Manager.UI.CurrentScene as UISceneMain;
    }

    public List<BaseEnemy> GetEnemyList()
    {
        return enemyList;
    }

    public StageUIData UITextReturn()
    {
        var UITable = stageUITable.Select(x => x.Key).ToList();
        var CurChapter = UITable.OrderBy(x => (x - Chapter <= 0)).Last();
        stageUITable.TryGetValue(CurChapter, out var chapterUI);
        return chapterUI;
    }

    #endregion

    #region Stage Progress

    public void BattleStart()
    {
        AudioBGM.Instance.VolumeBGMScale = 0.1f;
        AudioBGM.Instance.Play(Manager.Address.GetAudioBGM("testbgm"));

        stageCoroutine ??= CoroutineHelper.StartCoroutine(TestBattleCycle());
    }

    public void BattleStop()
    {
        CoroutineHelper.StopCoroutine(stageCoroutine);
        stageCoroutine = null;
    }

    public void StageFailed()
    {
        BattleStop();
        EnemyReset();
        
        // 보스 잡다 죽었으면 루프랑 버튼 켜주고 진행도만 하나 뒤로 물리기
        if (BossAppearance)
        {
            WaveLoop = true;
            StageLevel--;
            uISceneMain.RetryBossButtonToggle();
            uISceneMain.WaveLoopImageToggle();
        }
        else if (StageLevel > 0)
        {
            // 스테이지 진행 중 죽었으면 진행도만 하나 낮추기
            StageLevel--;
        }
        else
        {
            // 스테이지 처음에서 죽었으면 아예 뒤로 물리기
            Chapter--;
            StageLevel = 3;

            ChapterCheck();
            StageDataChange(Chapter);
            Manager.Game.Player.IdleRewardPopupUpdate();
        }

        uISceneMain.UpdateCurrentStage();
        uISceneMain.UpdateStageLevel(StageLevel);
        BattleStart();
    }

    // [임시] 코루틴 => 전투 무한 사이클
    private IEnumerator TestBattleCycle()
    {
        while (true)
        {
            //Debug.Log($"Difficulty : {Difficulty}, CurrentStage : {StageChapter}, StageProgress : {StageLevel}");
            
            // #1. 스폰 전 스테이지를 클리어하고 넘어왔으면 체력 리셋
            if (PlayerReset)
            {
                var Player = Manager.Game.Player;
                Player.SetCurrentHp(Player.ModifierHp);
                PlayerReset = false;
            }

            // #2. 시작 후 0.5초 뒤 적 웨이브 스폰
            yield return new WaitForSeconds(0.5f);
            if (!BossAppearance)
            {
                // 총 3초동안 몬스터 나오도록 하기. 테이블로 빼는것도 생각해봐야
                var delay = 3.0f / EnemySpawnCount;
                for (int i = 0; i < EnemySpawnCount; i++)
                {
                    yield return new WaitForSeconds(delay);
                    EnemyWaveSpawn();
                }
            }
            else
            {
                BossWaveSpawn();
            }

            // #3. 웨이브 클리어
            yield return new WaitUntil(()=> enemyList.Count == 0);
            WaveCompleted();
        }
    }

    private void EnemyWaveSpawn()
    {
        // 랜덤으로 Enemy 설계도 선정
        var randomEnemyName = StageConfig.Enemies[Random.Range(0, StageConfig.Enemies.Length)];
        var enemyBlueprint = Manager.Address.GetBlueprint(randomEnemyName) as EnemyBlueprint;

        // BaseEnemy 랜덤 Y축 위치 선정
        var randomYPos = Random.Range(spawnPoint[0].position.y, spawnPoint[1].position.y);
        var randomPos = new Vector2(spawnPoint[0].position.x, Mathf.Round(randomYPos * 10.0f) * 0.1f);

        // BaseEnemy 오브젝트 생성
        var enemyObject = Manager.ObjectPool.GetGo("EnemyFrame");

        // 레이어 조정
        var enemySprite = enemyObject.GetComponent<SpriteRenderer>();
        enemySprite.sortingOrder = (int)Mathf.Ceil(spawnPoint[0].position.y * 10.0f - (randomPos.y * 10.0f));
        var enemy = enemyObject.GetComponent<BaseEnemy>();
        // 적 설정
        enemy.SetEnemy(enemyBlueprint, randomPos, EnemyStatRate, EnemyGoldRate);
        enemyList.Add(enemy);

        // 보스->몬스터 임시 변경
        enemyObject.transform.localScale = new Vector2(1 - ratio, 1 - ratio);
    }

    private void BossWaveSpawn()
    {
        uISceneMain.StageLevelGaugeToggle(false);

        // Boss 설계도 가져오기
        var enemyBlueprint = Manager.Address.GetBlueprint(StageConfig.Boss) as EnemyBlueprint;
        var bossObject = Manager.ObjectPool.GetGo("EnemyFrame");
        var enemy = bossObject.GetComponent<BaseEnemy>();
        enemy.SetEnemy(enemyBlueprint, bossSpawnPoint.position, EnemyStatRate, EnemyGoldRate);
        enemyList.Add(enemy);

        // 보스 설정 임시 변경
        bossObject.transform.localScale = new Vector2(3 - ratio, 3 - ratio);
    }

    private void WaveCompleted()
    {
        // 스테이지가 루프모드면 진행도가 증가하지 않음
        if (!WaveLoop)        
            StageLevel++;
        
        // 보스 처치 시 챕터 상승
        if (StageClear)
        {
            StageLevel = 0;
            PlayerReset = true;

            Chapter++;
            ChapterCheck();
            StageDataChange(Chapter);
            Manager.Game.Player.IdleRewardPopupUpdate();
            uISceneMain.StageLevelGaugeToggle();
            backgroundControl.ChangeSprite(); // 챕터 상승 시 배경 변경
        }

        uISceneMain.UpdateStageLevel(StageLevel);
        SaveStage();

        Manager.Quest.QuestDB[3].currentValue = Chapter; // 스테이지 퀘스트 달성 값 변경
    }

    private void EnemyReset()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject.Destroy(enemyList[i].gameObject);
        }
        enemyList.Clear();
    }

    public void RetryBossBattle()
    {
        BattleStop();
        EnemyReset();
        uISceneMain.RetryBossButtonToggle();
        uISceneMain.WaveLoopImageToggle();

        WaveLoop = false;
        StageLevel++;

        BattleStart();
    }

    private void ChapterCheck()
    {
        if (Chapter == 0)
        {
            Chapter = 1;
            StageLevel = 0;
        }
        
        if (Chapter > stageTable.Count)
        {
            Chapter = stageTable.Count;
        }
    }

    private void SaveStage()
    {
        uISceneMain.UpdateCurrentStage();

        // 데이터 저장
        Manager.Data.Save();
    }

    #endregion
}

#region Table Serializable Class

[System.Serializable]
public class StageDataTable
{
    public List<StageData> stageDataTable;
}

[System.Serializable]
public class StageData
{
    public int Index;
    public string StageConfig;
    public string StageBackground;
    public int EnemyStatRate;
    public int EnemyGoldRate;
    public int EnemySpawnCount;
    public int IdleGoldReward;
}

[System.Serializable]
public class StageUIDataTable
{
    public List<StageUIData> stageUIDataTable;
}

[System.Serializable]
public class StageUIData
{
    public int Index;
    public string UIText;
}

#endregion