using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneKDH : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        // 씬 진입 시 처리
        Manager.UI.ShowScene<UISceneTest>();

        return true;
    }
}
