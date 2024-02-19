public class UIPopup : UIBase
{
    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvasPopup(gameObject);
    }
}
