using UnityEngine;

public class MainScene : BaseScene
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoint;


    public void SceneStart()
    {
        // 플레이어 위치 & UI 생성
        Manager.Game.PlayerInit(playerSpawnPoint.position);
        Manager.UI.ShowScene<UISceneMain>();
        Manager.Summon.SetSummon();

        // 스테이지 전투 구성
        Manager.Stage.SetStage(enemySpawnPoint, BossSpawnPointAdd());
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
