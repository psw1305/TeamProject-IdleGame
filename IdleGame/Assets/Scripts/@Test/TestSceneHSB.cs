using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneHSB : BaseScene
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform enemySpawnPoint;
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Manager.UI.ShowScene<UISceneTest>();
        Manager.Game.SetPosition(playerSpawnPoint.position);

        // 스테이지 전투 구성 & 시작
        Manager.Stage.Initialize();
        Manager.Stage.SetSpawnPoint(enemySpawnPoint);
        Manager.Stage.BattleStart();

        return true;
    }
}
