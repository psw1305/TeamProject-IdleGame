using UnityEngine;

public class TestScenePSW : BaseScene
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoint;

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        // 데이터 초기화
        Manager.Data.Initialize("test-user-kdh");

        // 보스 임시 스폰 포인트 스크립트로 만들기
        TestBossSpawnPointAdd(out Transform bossSpawnPoint);

        Manager.UI.ShowScene<UISceneMain>();
        Manager.Game.SetPosition(playerSpawnPoint.position);

        // 스테이지 전투 구성 & 시작
        Manager.Stage.Initialize();
        Manager.Stage.SetSpawnPoint(enemySpawnPoint);
        Manager.Stage.SetBossPoint(bossSpawnPoint);
        Manager.Stage.BattleStart();

        return true;
    }

    private void TestBossSpawnPointAdd(out Transform bossSpawnPosition)
    {
        var spawnPointTransform = this.transform.Find("Enemy Spawn Point");
        var bossSpawnPoint = Instantiate(new GameObject("Boss Spawn Point"), spawnPointTransform.position, Quaternion.identity);
        bossSpawnPosition = bossSpawnPoint.transform;
        bossSpawnPoint.transform.position = new Vector2(3.5f, 1.5f);
        bossSpawnPoint.transform.parent = spawnPointTransform;
    }
}
