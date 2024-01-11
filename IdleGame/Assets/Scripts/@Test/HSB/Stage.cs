using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private StageBlueprint stageConfig;

    public int StageProgress { get; private set; }

    public bool StageClear => StageProgress >= stageConfig.BattleCount;

    [SerializeField] private List<Enemy> enemyList = new List<Enemy>();

    public void InitStage()
    {
        // 일단 SO 로드가 따로 없어서 임시 로드
        stageConfig = Resources.Load<StageBlueprint>("ScriptableObjects/StageConfig");
    }
    private void Start()
    {
        StageProgress = 0;
        MonsterSpawn();
        StartCoroutine(TestAutoEnemyRemove());
    }

    private void Update()
    {
        if (enemyList.Count == 0)
        {
            if (!StageClear)
            {
                StageProgress++;
                Debug.Log(StageProgress);
                MonsterSpawn();
            }
            else
            {
                MonsterSpawn();
                ApplyStageCount();
            }
        }
    }

    private void MonsterSpawn()
    {
        EnemyBlueprint enemyBlueprint = Resources.Load<EnemyBlueprint>("ScriptableObjects/TestEnemy");

        for (int i = 0; i < 5; i++) // 나오는 횟수 임시로 5회
        {
            int temp = Random.Range(0, 2);
            Vector2 randomPos = new Vector2(0, Random.Range(-2.0f, 2.0f));
            Enemy enemy = Instantiate(stageConfig.Enemies[temp], randomPos, Quaternion.identity);
            enemy.SetEnemy(enemyBlueprint);
            enemyList.Add(enemy);
        }
    }

    private void ApplyStageCount()
    {
        StageProgress = 0;

        Debug.Log("Stage Clear!");
    }

    IEnumerator TestAutoEnemyRemove()
    {
        while (true)
        {
            if (enemyList.Count > 0)
            {
                Object.Destroy(enemyList[0].gameObject);
                enemyList.RemoveAt(0);
            }
            enemyList.RemoveAll(enemy => enemy == null);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

