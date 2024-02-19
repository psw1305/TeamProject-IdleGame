using UnityEngine;

public class MainScene : MonoBehaviour
{
    #region Fields

    [SerializeField] private UITopScene uiTopMain;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoint;
    private bool isLoadComplete = false;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        StartCoroutine(
        Manager.Asset.LoadAllAsyncCoroutine<Object>("Bundle", (key, count, totalCount) =>
        {
            uiTopMain.UpdateLoading(count, totalCount);

            if (count >= totalCount)
            {
                isLoadComplete = true;
                uiTopMain.UpdateLoadingComplete();
            }
        }));
    }

    private void OnApplicationQuit()
    {
        if (isLoadComplete) Manager.Data.Save();
    }

    #endregion

    #region Scene Setting

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

    #endregion
}
