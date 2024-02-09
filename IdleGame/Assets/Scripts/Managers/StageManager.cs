using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager
{
    #region Fields

    // json 경로 관련
    private string _tableText;

    private Dictionary<int, StageData> stageTable;
    private StageData _stageData;
    private List<BaseEnemy> enemyList = new();
    private Transform[] spawnPoint;
    private Transform bossSpawnPoint;
    private Coroutine _stageCoroutine;
    private UISceneMain _uISceneMain;

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
    public string DifficultyStr => _stageData.Difficulty;
    public StageBlueprint StageConfig => Manager.Assets.GetBlueprintStage(_stageData.StageConfig) as StageBlueprint;
    public string StageBackground => string.Empty;
    public int EnemyStatRate => _stageData.EnemyStatRate;
    public int EnemyGoldRate => _stageData.EnemyGoldRate;
    public int EnemySpawnCount => _stageData.EnemySpawnCount;

    #endregion

    #region Table Reference

    private void StageDataChange(int index)
    {
        stageTable.TryGetValue(index, out var data);
        _stageData = data;
    }

    #endregion

    #region Init

    public void Initialize()
    {
        // json 파일 로딩, 딕셔너리에 인덱스 그룹 넣기
        _tableText = Manager.Assets.GetTextItem("ItemTableStage");
        var stageDataTable = JsonUtility.FromJson<StageDataTable>($"{{\"stageDataTable\":{_tableText}}}");

        stageTable = stageDataTable.stageDataTable
            .ToDictionary(group => group.Index, group => group);
    }

    public void SetStage(GameUserProfile profile)
    {
        Chapter = profile.Stage_Chapter;
        StageLevel = profile.Stage_Level;
        WaveLoop = profile.Stage_WaveLoop;

        // 씬 -> 세션 생성 -> 게임 시작 순서. 여기서 스테이지 정보 및 UI 갱신
        StageDataChange(Chapter);
        _uISceneMain = Manager.UI.CurrentScene as UISceneMain;
    }

    public void SetSpawnPoint(Transform[] spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public void SetBossPoint(Transform bossSpawnPoint)
    {
        this.bossSpawnPoint = bossSpawnPoint;
    }

    public List<BaseEnemy> GetEnemyList()
    {
        return enemyList;
    }

    #endregion

    #region Stage Progress

    public void BattleStart()
    {
        _stageCoroutine ??= CoroutineHelper.StartCoroutine(TestBattleCycle());
    }

    public void BattleStop()
    {
        CoroutineHelper.StopCoroutine(_stageCoroutine);
        _stageCoroutine = null;
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
            _uISceneMain.RetryBossButtonToggle();
            _uISceneMain.WaveLoopImageToggle();
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
            StageDataChange(Chapter);

            StageLevel = 3;
        }

        _uISceneMain.UpdateCurrentStage();
        _uISceneMain.UpdateStageLevel(StageLevel);
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
        var enemyBlueprint = Manager.Assets.GetBlueprintEnemy(randomEnemyName) as EnemyBlueprint;

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
        enemyObject.transform.localScale = new Vector2(1, 1);
    }

    private void BossWaveSpawn()
    {
        _uISceneMain.StageLevelGaugeToggle(false);

        // Boss 설계도 가져오기
        var enemyBlueprint = Manager.Assets.GetBlueprintEnemy(StageConfig.Boss) as EnemyBlueprint;
        var bossObject = Manager.ObjectPool.GetGo("EnemyFrame");
        var enemy = bossObject.GetComponent<BaseEnemy>();
        enemy.SetEnemy(enemyBlueprint, bossSpawnPoint.position, EnemyStatRate, EnemyGoldRate);
        enemyList.Add(enemy);

        // 보스 설정 임시 변경
        bossObject.transform.localScale = new Vector2(3, 3);
    }

    private void WaveCompleted()
    {
        // 스테이지가 루프모드면 진행도가 증가하지 않음
        if (!WaveLoop)        
            StageLevel++;
        
        // 마지막 진행은 보스를 등장시킴
        if (StageClear)
        {
            StageLevel = 0;
            PlayerReset = true;

            Chapter++;
            StageDataChange(Chapter);
            _uISceneMain.StageLevelGaugeToggle();
        }

        _uISceneMain.UpdateStageLevel(StageLevel);
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
        _uISceneMain.RetryBossButtonToggle();
        _uISceneMain.WaveLoopImageToggle();

        WaveLoop = false;
        StageLevel++;

        BattleStart();
    }

    private void SaveStage()
    {
        _uISceneMain.UpdateCurrentStage();

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
    public string Difficulty;
    public string Chapter;
    public string StageConfig;
    public string StageBackground;
    public int EnemyStatRate;
    public int EnemyGoldRate;
    public int EnemySpawnCount;
}

#endregion