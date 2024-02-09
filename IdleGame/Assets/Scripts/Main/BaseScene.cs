using System.Collections;
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
        if (obj == null) Manager.Assets.InstantiateUI("eventsystem").name = "@EventSystem";

        initialized = true;
        return true;
    }

    #region Unity Flow

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private IEnumerator Start()
    {
        // #1. 동기 작업 => 파일 불러오기, 데이터 로드
        yield return StartCoroutine(Manager.Assets.DownloadFiles());
        yield return StartCoroutine(Manager.Data.Load());

        // #2. 비동기 작업 => 동시에 처리
        Manager.Resource.Initialize();
        Manager.Game.Initialize();
        Manager.ObjectPool.Initialize();
        Manager.Ranking.Initialize();
        Initialize();
    }

    private void OnApplicationQuit()
    {
        Manager.Data.Save();
    }

    #endregion
}
