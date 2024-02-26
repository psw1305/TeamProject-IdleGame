using UnityEngine;

public class UIScene : UIBase
{
    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvasScene(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Manager.UI.CheckPopupStack()) return;
            
            if (Manager.UI.CheckSceneStack())
            {
                Manager.UI.CloseSubScene();
                Manager.UI.Top.SetCloseButton(false);
            }
            else
            {
                // 게임종료 팝업 생성
            }
        }
    }
}
