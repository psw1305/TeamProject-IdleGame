using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager
{
    #region Fields

    private int maxStage = 5;   // 임시 변수
    private StageBlueprint stageConfig;
    private List<BaseEnemy> enemyList = new();
    private Transform[] spawnPoint;
    private StageBlueprint[] stageBlueprints;

    #endregion

    #region Properties

    public int Difficulty {  get; private set; }
    public int CurrentStage { get; private set; }
    public int StageProgress { get; private set; }
    public bool StageClear => StageProgress > stageConfig.BattleCount;
    public bool WaveClear => enemyList.Count == 0;
    // 스테이지 클리어 시 임시 강화율
    public int EnemyStatRate { get; private set; }
    public int StageRewardRate { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        CurrentStage = 0;
        Difficulty = 1;
        EnemyStatRate = 1;
        StageRewardRate = 1;
        stageBlueprints = new StageBlueprint[maxStage];

        // 스테이지 블루 프린트 미리 로드해두기
        for (int i = 0; i < maxStage; i++)
        {
            var stageConfig = string.Concat("StageConfig_", i+1);
            stageBlueprints[i] = Manager.Resource.GetBlueprint(stageConfig) as StageBlueprint;
        }
        stageConfig = stageBlueprints[CurrentStage];
        StageProgress = 0;
    }

    public void SetSpawnPoint(Transform[] spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public List<BaseEnemy> GetEnemyList()
    {
        return enemyList;
    }

    #endregion

    #region Stage Progress

    public void BattleStart()
    {
        CoroutineHelper.StartCoroutine(TestBattleCycle());
    }

    // [임시] 코루틴 => 전투 무한 사이클
    private IEnumerator TestBattleCycle()
    {
        while (true)
        {
            // #1. 시작 후 1초 뒤 적 웨이브 스폰
            yield return new WaitForSeconds(1.0f);
            EnemyWaveSpawn();

            yield return new WaitForSeconds(3000.0f);

            // #2. 적 라이프 타임 3초뒤 파괴
            for (int i = 0; i < enemyList.Count; i++)
            {
                var enemyIdx = Random.Range(0, enemyList.Count);
                GameObject.Destroy(enemyList[enemyIdx].gameObject);
                yield return new WaitForSeconds(0.2f);
                enemyList.Remove(enemyList[enemyIdx]);
                yield return new WaitForSeconds(0.2f);
            }

            // #3. 웨이브 클리어
            yield return new WaitUntil(()=> enemyList.Count == 0);
            WaveCompleted();
        }
    }

    private void EnemyWaveSpawn()
    {
        //Debug.Log(StageProgress);

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
            //enemy.SetStatWeight(EnemyStatRate);
            //enemy.SetReward(StageRewardRate);
            enemyList.Add(enemy);
        }
    }

    private void WaveCompleted()
    {
        StageProgress++;
        
        if (StageClear)
        {
            StageProgress = 0;

            // 현재 스테이지 값 증가 시켜 주고, 증가 후 스테이지가 최대치에 도달하면 난이도 올리고 1로 되돌아가기
            CurrentStage++;
            if (CurrentStage == maxStage)
            {
                CurrentStage = 0;
                Difficulty++;
            }
                
            // 스테이지 정보를 다시 현재 스테이지값에 맞춰 변경해주고 스텟 상승량 변경
            stageConfig = stageBlueprints[CurrentStage];
            EnemyStatRate += 1 * Difficulty;
            StageRewardRate += 1 + (Difficulty / 2);
            Debug.Log($"EnemyStatRate : {EnemyStatRate}, StageRewardRate : {StageRewardRate}");
        }
    }

    #endregion
}

