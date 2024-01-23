using UnityEngine;

public class TestSceneKHK : BaseScene
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
        Manager.Inventory.SetDataPath("/Scripts/Json/Tester/KHK/InvenDB_KHK.json");

        // 퀘스트 json 파일 경로 설정
        Manager.Quest.SetDataPath("/Scripts/Json/Tester/KHK/QuestDB_KHK.json");

        // 세션 생성 후 => 전투 시작
        Manager.Session.Initialize("test-khk");

        return true;
    }

    private void TestBossSpawnPointAdd(out Transform bossSpawnPosition)
    {
        var spawnPointTransform = this.transform.Find("_Enemy Spawn Point");
        var bossSpawnPoint = Instantiate(new GameObject("Boss Spawn Point"), spawnPointTransform.position, Quaternion.identity);
        bossSpawnPosition = bossSpawnPoint.transform;
        bossSpawnPoint.transform.position = new Vector2(3.5f, 1.5f);
        bossSpawnPoint.transform.parent = spawnPointTransform;
    }
}
