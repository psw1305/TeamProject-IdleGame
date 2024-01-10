public class TestScenePSW : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;

        // 씬 진입 시 처리
        Manager.UI.ShowScene<UISceneTest>();

        return true;
    }
}
