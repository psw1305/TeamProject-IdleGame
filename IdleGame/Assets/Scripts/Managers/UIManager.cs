using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    #region Fields

    private int order = 10;
    private Stack<UIPopup> popupStack = new();

    #endregion

    #region Properties

    public GameObject UIRoot
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root") ?? new GameObject("@UI_Root");
            return root;
        }
    }
    public UIScene SceneUI { get; private set; }

    #endregion

    #region Init

    /// <summary>
    /// Scene, Popup 생성 => 캔버스 초기화
    /// </summary>
    /// <param name="uiObject">해당 UI 오브젝트</param>
    public void SetCanvas(GameObject uiObject)
    {
        // Canvas 컴포넌트 세팅
        var canvas = Utility.GetOrAddComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = order++;

        // Canvas Scaler 세팅
        var canvasScaler = Utility.GetOrAddComponent<CanvasScaler>(uiObject);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);
    }

    #endregion

    #region Scene

    public T ShowScene<T>(GameObject sceneObject) where T : UIScene
    {
        GameObject scene = GameObject.Instantiate(sceneObject, UIRoot.transform);
        T sceneUI = Utility.GetOrAddComponent<T>(scene);
        SceneUI = sceneUI;
        return sceneUI;
    }

    #endregion

    #region Popup

    public T ShowPopup<T>(GameObject popupObject) where T : UIPopup
    {
        GameObject obj = GameObject.Instantiate(popupObject, UIRoot.transform);
        T popup = Utility.GetOrAddComponent<T>(obj);
        popupStack.Push(popup);

        return popup;
    }

    public void ClosePopup(UIPopup popup)
    {
        if (popupStack.Count == 0) return;

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopup();
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;

        UIPopup popup = popupStack.Pop();
        GameObject.Destroy(popup.gameObject);
        order--;
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopup();
        }
    }

    #endregion
}
