using UnityEngine;

public class UIPopup : UIBase
{
    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvasPopup(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.ClosePopup();
        }
    }
}
