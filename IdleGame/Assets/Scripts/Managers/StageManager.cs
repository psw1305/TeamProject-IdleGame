using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager
{
    #region Fields

    private int maxStage = 5;   // 임시 변수
    private StageBlueprint stageConfig;
    private List<BaseEnemy> enemyList = new();
    private Transform[] spawnPoint;
    private Transform bossSpawnPoint;
    private StageBlueprint[] stageBlueprints;
    private Coroutine _stageCoroutine;
    private Button _bossButton;

    #endregion

    #region Properties

    // 플레이 데이터 프로퍼티
    public int Difficulty {  get; private set; }
    public int Chapter { get; private set; }
    public int StageLevel { get; private set; }
    public bool WaveLoop { get; private set; }

    // 관리용 프로퍼티
    public bool BossAppearance => StageLevel == stageConfig.BattleCount;
    public bool StageClear => StageLevel > stageConfig.BattleCount;
    public bool WaveClear => enemyList.Count == 0;
    // 스테이지 클리어 시 임시 강화율
    public int EnemyStatRate { get; private set; }
    public int StageRewardRate { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        EnemyStatRate = 1;
        StageRewardRate = 1;
        //BossAppearance = false;
        stageBlueprints = new StageBlueprint[maxStage];

        // 스테이지 설계도 미리 로드해두기
        for (int i = 0; i < maxStage; i++)
        {
            var stageConfig = string.Concat("StageConfig_", i+1);
            stageBlueprints[i] = Manager.Resource.GetBlueprint(stageConfig) as StageBlueprint;
        }
        stageConfig = stageBlueprints[Chapter];
    }

    public void SetStage(GameUserProfile profile)
    {
        Difficulty = profile.Stage_Difficulty;
        Chapter = profile.Stage_Chapter;
        StageLevel = profile.Stage_Level;
        WaveLoop = profile.Stage_WaveLoop;
    }

    public void SetSpawnPoint(Transform[] spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public void SetBossPoint(Transform bossSpawnPoint)
    {
        this.bossSpawnPoint = bossSpawnPoint;
    }

    public void SetRetryBossButton(Button button)
    {
        _bossButton = button;
        _bossButton.gameObject.SetActive(false);
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
            RetryBossButtonToggle();
        }
        else
        {
            // 스테이지 진행중 죽었으면 스테이지 뒤로 물리기, 맨 처음 스테이지면 난이도를 뒤로 무르고 마지막 스테이지로
            EnemyStatRate -= 1 * Difficulty;
            StageRewardRate -= 1 + (Difficulty / 2);
            Chapter--;
            if (Chapter < 0)
            {
                Chapter = maxStage - 1;
                Difficulty--;
            }
            stageConfig = stageBlueprints[Chapter];

            StageLevel = 0;
        }

        BattleStart();
    }

    // [임시] 코루틴 => 전투 무한 사이클
    private IEnumerator TestBattleCycle()
    {
        while (true)
        {
            //Debug.Log($"Difficulty : {Difficulty}, CurrentStage : {StageChapter}, StageProgress : {StageLevel}");
            
            // #1. 시작 후 1초 뒤 적 웨이브 스폰
            yield return new WaitForSeconds(1.0f);
            EnemyWaveSpawn();

            // #2. 웨이브 클리어
            yield return new WaitUntil(()=> enemyList.Count == 0);
            WaveCompleted();
        }
    }

    private void EnemyWaveSpawn()
    {
        // 웨이브 진행 횟수가 StageConfig에 도달하지 못하면 잡몹 소환
        if (!BossAppearance)
        {
            for (int i = 0; i < 5; i++)
            {
                // 랜덤으로 Enemy 설계도 선정
                var randomEnemyName = stageConfig.Enemies[Random.Range(0, stageConfig.Enemies.Length)];
                var enemyBlueprint = Manager.Resource.GetBlueprint(randomEnemyName) as EnemyBlueprint;

                // BaseEnemy 랜덤 Y축 위치 선정
                var randomIdx = Random.Range(0, spawnPoint.Length);
                var randomPos = new Vector2(spawnPoint[randomIdx].position.x, spawnPoint[randomIdx].position.y);

                // BaseEnemy 오브젝트 생성
                var enemyObject = Manager.Resource.InstantiatePrefab("EnemyFrame");
                var enemy = enemyObject.GetComponent<BaseEnemy>();
                enemy.SetEnemy(enemyBlueprint);
                enemy.SetPosition(randomPos);
                enemy.SetStatWeight(EnemyStatRate);
                //enemy.SetReward(StageRewardRate);
                enemyList.Add(enemy);
            }
        }
        else
        {
            // Boss 설계도 가져오기
            var enemyBlueprint = Manager.Resource.GetBlueprint(stageConfig.Boss) as EnemyBlueprint;

            var bossObject = Manager.Resource.InstantiatePrefab("EnemyFrame");
            var enemy = bossObject.GetComponent<BaseEnemy>();
            enemy.SetEnemy(enemyBlueprint);
            enemy.SetPosition(bossSpawnPoint.position);
            enemyList.Add(enemy);

            // 보스 설정 임시 변경
            enemy.SetStatWeight(EnemyStatRate * 5);
            bossObject.transform.localScale = new Vector2(3, 3);
        }
    }

    private void WaveCompleted()
    {
        // 스테이지가 루프모드면 진행도가 증가하지 않음
        if (!WaveLoop)        
            StageLevel++;
        
        // 마지막 진행은 보스를 등장시킴
        if (StageClear)
        {
            StageLevel = 1;

            // 다음 챕터로 넘어가고, 스테이지가 최대치에 도달하면 난이도 올린뒤 처음으로 되돌아가기
            Chapter++;

            if (Chapter == maxStage)
            {                
                Chapter = 1;
                Difficulty++;
            }
                
            // 스테이지 정보를 다시 현재 스테이지값에 맞춰 변경해주고 스텟 상승량 변경
            stageConfig = stageBlueprints[Chapter];
            EnemyStatRate += 1 * Difficulty;
            StageRewardRate += 1 + (Difficulty / 2);
        }

        SaveStage();

        Manager.Quest.QuestDataBase.QuestDB[3].currentValue = Chapter; // 스테이지 퀘스트 달성 값 변경
    }

    private void EnemyReset()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject.Destroy(enemyList[i].gameObject);
        }
        enemyList.Clear();
    }

    public void RetryBossButtonToggle()
    {
        _bossButton.gameObject.SetActive(!_bossButton.IsActive());
    }

    public void RetryBossBattle()
    {
        BattleStop();
        EnemyReset();
        RetryBossButtonToggle();

        WaveLoop = false;
        StageLevel++;

        BattleStart();
    }

    private void SaveStage()
    {
        UISceneMain mainUI = Manager.UI.CurrentScene as UISceneMain; // 변수화 
        mainUI.UpdateCurrentStage();

        // 데이터 저장
    }

    #endregion
}

