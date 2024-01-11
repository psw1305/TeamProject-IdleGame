using UnityEngine;

public class TestSceneKHK : BaseScene
{
    public Player Player { get; private set; }    

    [SerializeField] private Transform spwanPoint;
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;
        // 씬 진입 시 처리


        // 플레이어 위치 생성
        var playerClone = Manager.Resource.InstantiatePrefab("TestPlayer", spwanPoint);
        Player = playerClone.GetComponent<Player>();

        return true;
    }
}
