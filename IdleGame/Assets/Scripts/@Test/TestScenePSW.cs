using UnityEngine;

public class TestScenePSW : BaseScene
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoint;

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        // 보스 임시 스폰 포인트 스크립트로 만들기
        TestBossSpawnPointAdd(out Transform bossSpawnPoint);
        Manager.Game.SetPosition(playerSpawnPoint.position);

        // 스테이지 전투 구성 & 시작
        Manager.Stage.Initialize();
        Manager.Stage.SetSpawnPoint(enemySpawnPoint);
        Manager.Stage.SetBossPoint(bossSpawnPoint);

        Manager.Summon.Initialize();

        // 아이템 DB json 파일 경로 설정
        Manager.Inventory.Initialize("InvenDB_PSW");

        // 세션 생성 후 => 전투 시작
        Manager.Session.Initialize("test-psw");

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
