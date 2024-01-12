using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    #region Fields

    private StageBlueprint stageConfig;
    private List<BaseEnemy> enemyList = new();
    private Transform spawnPoint;

    #endregion

    #region Properties

    public int StageProgress { get; private set; }
    public bool StageClear => StageProgress > stageConfig.BattleCount;
    public bool WaveClear => enemyList.Count == 0;

    #endregion

    #region Init

    public void Initialize()
    {
        stageConfig = Manager.Resource.GetBlueprint("StageConfig") as StageBlueprint;
        StageProgress = 0;
    }

    public void SetSpawnPoint(Transform spawnPoint)
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
            // #1. 적 웨이브 스폰
            EnemyWaveSpawn();

            yield return new WaitForSeconds(3.0f);

            // #2. 적 라이프 타임 3초뒤 파괴
            for (int i = 0; i < enemyList.Count; i++)
            {
                GameObject.Destroy(enemyList[i].gameObject);
            }
            enemyList.Clear();

            yield return new WaitForSeconds(1.0f);

            // #3. 웨이브 클리어
            WaveCompleted();
        }
    }

    private void EnemyWaveSpawn()
    {
        Debug.Log(StageProgress);

        for (int i = 0; i < 5; i++)
        {
            // 랜덤으로 Enemy 설계도 선정
            var randomEnemyName = stageConfig.Enemies[Random.Range(0, stageConfig.Enemies.Length)];
            var enemyBlueprint = Manager.Resource.GetBlueprint(randomEnemyName) as EnemyBlueprint;

            // BaseEnemy 랜덤 Y축 위치 선정
            var randomY = Random.Range(-1.0f, 1.0f);
            var randomPos = new Vector2(spawnPoint.position.x, spawnPoint.position.y + randomY);

            // BaseEnemy 오브젝트 생성
            var enemyObject = Manager.Resource.InstantiatePrefab("EnemyFrame");
            var enemy = enemyObject.GetComponent<BaseEnemy>();
            enemy.SetEnemy(enemyBlueprint);
            enemy.SetPosition(randomPos);
            enemyList.Add(enemy);
        }
    }

    private void WaveCompleted()
    {
        StageProgress++;
        
        if (StageClear)
        {
            StageProgress = 0;

            // 추후 전투 클리어 작성
            Debug.Log("Stage Clear!");
        }
    }

    #endregion
}

