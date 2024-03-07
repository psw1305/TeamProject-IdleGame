using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    #region Fields

    private int sceneOrder = 10;
    private int popupOrder = 30;
    private readonly Stack<UIScene> sceneStack = new();
    private readonly Stack<UIPopup> popupStack = new();

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
    public UITopScene Top { get; private set; }
    public UIScene CurrentScene { get; private set; }
    public UIScene CurrentSubScene { get; private set; }
    public UIPopup CurrentPopup { get; private set; }
    public UIPopupRewardsSummon CurrentSummonPopup { get; private set; }

    #endregion

    #region Initialize

    /// <summary>
    /// Scene 생성 => 캔버스 초기화
    /// </summary>
    public void SetCanvasScene(GameObject uiObject)
    {
        // Canvas 컴포넌트 세팅
        var canvas = Utilities.GetOrAddComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sceneOrder++;

        // Canvas Scaler 세팅
        var canvasScaler = Utilities.GetOrAddComponent<CanvasScaler>(uiObject);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);
    }

    public void SetCanvasPopup(GameObject uiObject)
    {
        // Canvas 컴포넌트 세팅
        var canvas = Utilities.GetOrAddComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = popupOrder++;

        // Canvas Scaler 세팅
        var canvasScaler = Utilities.GetOrAddComponent<CanvasScaler>(uiObject);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 1920);
    }

    #endregion

    #region Scene

    public void InitTop(UITopScene uITopMain)
    {
        Top = uITopMain;
    }

    public T ShowScene<T>(string sceneName = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(sceneName)) sceneName = typeof(T).Name;

        GameObject obj = Manager.Asset.InstantiatePrefab(sceneName, UIRoot.transform);
        T scene = Utilities.GetOrAddComponent<T>(obj);
        CurrentScene = scene;

        return scene;
    }

    public T ShowSubScene<T>(string sceneName = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(sceneName)) sceneName = typeof(T).Name;

        if (CurrentSubScene != null)
        {
            CloseSubScene();
        }

        GameObject obj = Manager.Asset.InstantiatePrefab(sceneName, UIRoot.transform);
        T scene = Utilities.GetOrAddComponent<T>(obj);
        CurrentSubScene = scene;
        sceneStack.Push(scene);

        return scene;
    }

    public void CloseSubScene()
    {
        if (sceneStack.Count == 0) return;

        UIScene scene = sceneStack.Pop();
        Destroy(scene.gameObject);
        sceneOrder--;
    }

    public bool CheckSceneStack()
    {
        if (sceneStack.Count != 0) return true;

        return false;
    }

    #endregion

    #region Popup

    public T ShowPopup<T>(string popupName = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(popupName)) popupName = typeof(T).Name;

        GameObject obj = Manager.Asset.InstantiatePrefab(popupName, UIRoot.transform);
        T popup = Utilities.GetOrAddComponent<T>(obj);
        CurrentPopup = popup;
        popupStack.Push(popup);

        return popup;
    }

    public T ShowSummonPopup<T>(string popupName = null) where T : UIPopupRewardsSummon
    {
        if (string.IsNullOrEmpty(popupName)) popupName = typeof(T).Name;

        GameObject obj = Manager.Asset.InstantiatePrefab(popupName, UIRoot.transform);
        T summonPopup = Utilities.GetOrAddComponent<T>(obj);
        CurrentSummonPopup = summonPopup;
        popupStack.Push(summonPopup);

        return summonPopup;
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;

        UIPopup popup = popupStack.Pop();
        Destroy(popup.gameObject);
        popupOrder--;
    }

    public void CloseCurrentSummonPopup()
    {
        if (CurrentSummonPopup == null) return;

        UIPopup popup = popupStack.Pop();
        Destroy(popup.gameObject);
        popupOrder--;
    }


    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopup();
        }
    }

    public bool CheckPopupStack()
    {
        if (popupStack.Count != 0) return true;

        return false;
    }

    #endregion

    #region Elements

    public T AddElement<T>(string elementName = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(elementName)) elementName = typeof(T).Name;

        GameObject obj = Manager.Asset.InstantiatePrefab(elementName, UIRoot.transform);
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
