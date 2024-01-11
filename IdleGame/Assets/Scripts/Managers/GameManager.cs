public class GameManager
{
    #region

    public Player Player { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        // TODO => Player 리소스 폴더에서 가져와서 생성 후 초기화
        // 스테이지 정보를 가져와서 전투 구현
        var playerClone = Manager.Resource.InstantiatePrefab("PlayerModel");
        Player = playerClone.GetComponent<Player>();
    }

    #endregion
}
