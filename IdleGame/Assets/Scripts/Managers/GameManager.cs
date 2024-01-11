using UnityEngine;

public class GameManager
{
    #region

    public Player Player { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        var playerClone = Manager.Resource.InstantiatePrefab("PlayerFrame");
        Player = playerClone.GetComponent<Player>();
    }

    #endregion

    #region Set Player

    public void SetPosition(Vector2 position)
    {
        Player.transform.position = position;
    }

    #endregion
}
