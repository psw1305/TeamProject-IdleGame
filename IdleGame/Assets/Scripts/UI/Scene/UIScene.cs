public class UIScene : UIBase
{
    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvasScene(gameObject);
    }
}
