using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISceneTest : UIScene
{
    #region Fields

    [SerializeField] private Transform testTransform;
    [SerializeField] private Image testImage;
    [SerializeField] private TextMeshProUGUI testText;
    [SerializeField] private Button testButton;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();

        SetObjects();
        SetImages();
        SetTexts();
        SetButtons();
        SetEvents();

        // 씬 내에서 강화 버튼 초기화
        // 해당 버튼을 누르면 플레이어가 강화
    }

    private void SetObjects()
    {
        SetUI<Transform>();
        testTransform = GetUI<Transform>("Transform_Test");
    }

    private void SetImages()
    {
        SetUI<Image>();
        testImage = GetUI<Image>("Image_Test");
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        testText = GetUI<TextMeshProUGUI>("Text_Test");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        testButton = GetUI<Button>("Button_Test");
    }

    private void SetEvents()
    {
        testButton.gameObject.SetEvent(UIEventType.Click, TestClick);
    }

    #endregion

    #region Button Events

    private void TestClick(PointerEventData eventData)
    {
        Debug.Log("test");
    }

    #endregion
}
