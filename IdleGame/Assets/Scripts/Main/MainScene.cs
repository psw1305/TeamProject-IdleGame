using UnityEngine;

public class MainScene : BaseScene
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoint;

    public void SceneStart()
    {
        //보스 임시 스폰 포인트 스크립트로 만들기

        // 스테이지 전투 구성
        Manager.Game.PlayerInit(playerSpawnPoint.position);
        Manager.UI.ShowScene<UISceneMain>();

        Manager.Stage.SetStage(enemySpawnPoint, BossSpawnPointAdd());
        Manager.Summon.SetSummon();

        Manager.Stage.BattleStart();
    }

    private Transform BossSpawnPointAdd()
    {
        var spawnPointTransform = this.transform.Find("Enemy Spawn Point");
        var bossSpawnPoint = Instantiate(new GameObject("Boss Spawn Point"), spawnPointTransform.position, Quaternion.identity);
        bossSpawnPoint.transform.position = new Vector2(3.5f, 1.5f);
        bossSpawnPoint.transform.parent = spawnPointTransform;

        return bossSpawnPoint.transform;
    }
}
