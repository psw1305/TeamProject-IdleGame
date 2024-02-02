using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    public UIScene UI { get; protected set; }
    private bool initialized = false;

    protected virtual bool Initialize()
    {
        if (initialized) return false;

        Object obj = GameObject.FindObjectOfType<EventSystem>();
        if (obj == null) Manager.Resource.InstantiatePrefab("EventSystem").name = "@EventSystem";

        initialized = true;
        return true;
    }

    #region Unity Flow

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Manager.Resource.Initialize();
        Manager.Game.Initialize();
        Manager.ObjectPool.Initialize();

        // 플레이어 데이터 로드, 이후 플레이어 관련 데이터 불러오기 가능
        Manager.Data.Load();

        Manager.Ranking.Initialize();

        Initialize();
    }

    private void OnApplicationQuit()
    {
        Manager.Data.Save();
    }

    #endregion
}
