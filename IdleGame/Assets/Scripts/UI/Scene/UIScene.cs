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
                var alertPopup = Manager.UI.ShowPopup<UIPopupSystemAlert>();
                alertPopup.SetData(PopupAlertType.ApplicationQuit, GameQuit);
            }
        }
    }

    private void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }
}
