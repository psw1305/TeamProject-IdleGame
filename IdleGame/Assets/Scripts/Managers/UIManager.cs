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
            GameObject root = GameObject.Find("@UIRoot") ?? new GameObject("@UIRoot");
            return root;
        }
    }
    public UIScene CurrentScene { get; private set; }
    public UIPopup CurrentPopup { get; private set; }

    #endregion

    #region Init

    /// <summary>
    /// Scene, Popup 생성 => 캔버스 초기화
    /// </summary>
    /// <param name="uiObject">해당 UI 오브젝트</param>
    public void SetCanvas(GameObject uiObject)
    {
        // Canvas 컴포넌트 세팅
        var canvas = Utilities.GetOrAddComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = order++;

        // Canvas Scaler 세팅
        var canvasScaler = Utilities.GetOrAddComponent<CanvasScaler>(uiObject);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);
    }

    #endregion

    #region Scene

    public T ShowScene<T>(string sceneName = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(sceneName)) sceneName = typeof(T).Name;

        GameObject obj = Manager.Address.InstantiatePrefab(sceneName, UIRoot.transform);
        T scene = Utilities.GetOrAddComponent<T>(obj);
        CurrentScene = scene;

        return scene;
    }

    #endregion

    #region Popup

    public T ShowPopup<T>(string popupName = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(popupName)) popupName = typeof(T).Name;

        GameObject obj = Manager.Address.InstantiatePrefab(popupName, UIRoot.transform);
        T popup = Utilities.GetOrAddComponent<T>(obj);
        CurrentPopup = popup;
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
        Destroy(popup.gameObject);
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

    #region Elements

    public T AddElement<T>(string elementName = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(elementName)) elementName = typeof(T).Name;

        GameObject obj = Manager.Address.InstantiatePrefab(elementName, UIRoot.transform);
        T element = Utilities.GetOrAddComponent<T>(obj);

        return element;
    }

    private void Destroy(GameObject obj)
    {
        if (obj == null) return;
        UnityEngine.Object.Destroy(obj);
    }

    #endregion
}
