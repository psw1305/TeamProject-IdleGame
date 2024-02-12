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
        var playerClone = Manager.Address.InstantiatePrefab("PlayerFrame");
        Player = playerClone.GetComponent<Player>();
    }

    // TODO => 서순을 교체하니 실행이 가능함
    public void GameStart()
    {
        Manager.UI.ShowScene<UISceneMain>();

        Manager.Summon.SetSummon();
        Manager.Stage.SetStage(Manager.Data.Profile);
        Manager.Stage.BattleStart();

        AudioBGM.Instance.VolumeBGMScale = 0.1f;
        AudioBGM.Instance.Play(Manager.Address.GetAudioBGM("testbgm"));

        // 플레이어 데이터가 초기화 되는 부분
        Player.transform.position = playerPosition;
        Player.Initialize();
    }

    #endregion

    #region Set Player

    public void SetPosition(Vector2 position)
    {
        playerPosition = position;
    }

    #endregion
}
