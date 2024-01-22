using UnityEngine;

public class GameManager
{
    #region Fields

    private Vector2 playerPosition;

    #endregion

    #region Properties

    public Player Player { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        var playerClone = Manager.Resource.InstantiatePrefab("PlayerFrame");
        Player = playerClone.GetComponent<Player>();
    }

    public void GameStart()
    {
        // 플레이어 데이터가 초기화 되는 부분
        Player.transform.position = playerPosition;
        Player.Initialize();

        Manager.UI.ShowScene<UISceneMain>();
        Manager.Summon.SetSummon();
        Manager.Stage.SetStage(Manager.Data.Profile);
        Manager.Stage.BattleStart();
    }

    #endregion

    #region Set Player

    public void SetPosition(Vector2 position)
    {
        playerPosition = position;
    }

    #endregion
}
