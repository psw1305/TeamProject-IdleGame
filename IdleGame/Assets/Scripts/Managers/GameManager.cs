using UnityEngine;

public class GameManager
{
    #region Properties

    public Player Player { get; private set; }
    public MainScene Main { get; private set; }

    #endregion

    #region Init

    /// <summary>
    /// 데이터 동기화 후 => 게임 세팅
    /// </summary>
    public void Initialize()
    {
        Manager.Data.Load();

        var playerClone = Manager.Address.InstantiatePrefab("PlayerFrame");
        Player = playerClone.GetComponent<Player>();

        Manager.ObjectPool.Initialize();
        Manager.Ranking.Initialize();
        Manager.Stage.Initialize();
        Manager.Summon.Initialize();
        Manager.Inventory.Initialize();

        Main = GameObject.FindObjectOfType<MainScene>();
        Main.SceneStart();
    }

    // 플레이어 데이터가 초기화 되는 부분
    public void PlayerInit(Vector2 position)
    {
        Player.transform.position = position;
        Player.Initialize();
    }

    #endregion
}
