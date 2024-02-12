using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    public UIScene UI { get; protected set; }
    private bool initialized = false;

    protected virtual bool Initialize()
    {
        if (initialized) return false;

        Object obj = GameObject.FindObjectOfType<EventSystem>();
        if (obj == null) Manager.Address.InstantiatePrefab("EventSystem").name = "@EventSystem";

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
        // #1. 동기화 작업 => 순서대로 처리
        yield return StartCoroutine(DownloadFiles());
        yield return StartCoroutine(Manager.Data.Load());

        // #2. 비동기 작업 => 동시에 처리
        yield return null;
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

    #region File Download

    private IEnumerator DownloadFiles()
    {
        yield return Addressables.InitializeAsync();

        if (Manager.Address.Loaded)
        {
            Debug.Log("Loaded");
        }
        else
        {
            Manager.Address.Loaded = true;
            Manager.Address.LoadAllAsync<Object>("material", (key, count, totalCount) =>
            {
                //Debug.Log($"{count}/{totalCount}");

                //if (count >= totalCount)
                //{
                //    Debug.Log("first Load");
                //}
            });

            Manager.Address.LoadSprites("SpriteAtlas");

            Manager.Address.LoadAllAsync<Object>("default", (key, count, totalCount) =>
            {
            });
        }

        yield return null;
    }

    #endregion
}
