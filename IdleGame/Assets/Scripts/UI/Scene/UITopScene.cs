using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UITopScene : UIBase
{
    #region Serialize Fields

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject playingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject completeBtn;
    [SerializeField] private TextMeshProUGUI assetDownloadTxt;
    [SerializeField] private GameObject sceneCloseBtn;

    #endregion

    #region Fields

    private Player player;
    private TextMeshProUGUI txtGold;
    private TextMeshProUGUI txtGems;

    #endregion

    #region Playing Init

    private void InitTopUI()
    {
        SetTexts();
        SetButtons();

        player = Manager.Game.Player;
        Manager.UI.InitTop(this);

        UpdateGold();
        UpdateGems();
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        txtGold = GetUI<TextMeshProUGUI>("Txt_Gold");
        txtGems = GetUI<TextMeshProUGUI>("Txt_Gems");
    }

    private void SetButtons()
    {
        SetUI<Button>();
        SetButtonEvent("Btn_PlayerSystem", UIEventType.Click, OnPlayerSystem);
        SetButtonEvent("Btn_Ranking", UIEventType.Click, OnRanking);
        SetButtonEvent("Btn_Shop", UIEventType.Click, OnShop);
        SetButtonEvent("Btn_Close", UIEventType.Click, CloseSubScene);
    }

    public void SetCloseButton(bool active)
    {
        sceneCloseBtn.SetActive(active);
    }

    #endregion

    #region Button Events

    public void OnGameStart()
    {
        Manager.Data.Load();
        Manager.Game.Initialize();
        InitTopUI();

        loadingPanel.SetActive(false);
        playingPanel.SetActive(true);
    }

    private void OnPlayerSystem(PointerEventData eventData)
    {
        Manager.UI.ShowSubScene<UISubScenePlayerSystem>();
        sceneCloseBtn.transform.localPosition = new Vector2(-360, 0);
        sceneCloseBtn.SetActive(true);
    }
    
    private void OnRanking(PointerEventData eventData) => Debug.Log("랭킹시스템 임시 잠금");
    
    private void OnShop(PointerEventData eventData)
    {
        Manager.UI.ShowSubScene<UISubSceneShopSummon>();        
        sceneCloseBtn.transform.localPosition = new Vector2(360, 0);
        sceneCloseBtn.SetActive(true);

        if(PlayerPrefs.GetInt("ShopTutorial") == 0)
        {
            NPCManager.Instance.ActiveNPC("ShopTutorial");
            PlayerPrefs.SetInt("ShopTutorial", 1);
        }
        
    }

    private void CloseSubScene(PointerEventData eventData) 
    {
        Manager.UI.CloseSubScene();
        sceneCloseBtn.SetActive(false);
    }

    #endregion

    #region Update UI

    public void UpdateLoading(int count, int totalCount)
    {
        loadingSlider.value = (float)count / totalCount;
        assetDownloadTxt.text = $"에셋 다운로드중... ({count}/{totalCount})";
    }

    public void UpdateLoadingComplete()
    {
        loadingSlider.gameObject.SetActive(false);
        completeBtn.SetActive(true);

        assetDownloadTxt.text = "로딩이 완료됐습니다.";
    }

    public void UpdateGold()
    {
        txtGold.text = Utilities.ConvertToString(player.Gold);
    }

    public void UpdateGems()
    {
        txtGems.text = player.Gems.ToString();
    }

    #endregion
}
