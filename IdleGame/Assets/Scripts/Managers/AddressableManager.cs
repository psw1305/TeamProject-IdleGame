using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;

public class AddressableManager
{
    public bool Loaded { get; set; }
    private Dictionary<string, UnityEngine.Object> assets = new();
    private SpriteAtlas spriteAtlas;

    #region Work Flow

    public static string DownloadURL;
    private DownloadEvents events;
    private string labelToDownload;
    private long totalSize;
    private AsyncOperationHandle downloadHandle;

    public DownloadEvents InitializeSystem(string label, string downloadURL)
    {
        events = new DownloadEvents();

        Addressables.InitializeAsync().Completed += OnInitialized;

        labelToDownload = label;
        DownloadURL = downloadURL;

        ResourceManager.ExceptionHandler += OnException;

        return events;
    }

    public void Update()
    {
        if (Utilities.IsNetworkValid() == false)
        {
            // 네트워크가 끊길 시, 관련 메서드
        }

        if (downloadHandle.IsValid() 
            && downloadHandle.IsDone == false
            && downloadHandle.Status != AsyncOperationStatus.Failed)
        {
            var status = downloadHandle.GetDownloadStatus();

            long curDownloadedSize = status.DownloadedBytes;
            long remainedSize = totalSize - curDownloadedSize;

            events.NotifyDownloadProgress(
                new DownloadProgressStatus(
                    status.DownloadedBytes, 
                    totalSize, 
                    remainedSize, 
                    status.Percent));
        }
    }

    public void UpdateCatalog()
    {
        Addressables.CheckForCatalogUpdates().Completed += (result) =>
        {
            var catalogToUpdate = result.Result;
            if (catalogToUpdate.Count > 0)
            {
                Addressables.UpdateCatalogs(catalogToUpdate).Completed += OnCatelogUpdate;
            }
            else
            {
                events.NotifyCatalogUpdated();
            }
        };
    }

    public void DownloadSize()
    {
        Addressables.GetDownloadSizeAsync(labelToDownload).Completed += OnSizeDownloaded;
    }

    public void StartDownload()
    {
        downloadHandle = Addressables.DownloadDependenciesAsync(labelToDownload);
        downloadHandle.Completed += OnDependenciesDownloaded;
    }

    #endregion

    #region Init

    private void OnInitialized(AsyncOperationHandle<IResourceLocator> result)
    {
        events.NotifyInitialized();
    }

    private void OnCatelogUpdate(AsyncOperationHandle<List<IResourceLocator>> result)
    {
        events.NotifyCatalogUpdated();
    }

    private void OnSizeDownloaded(AsyncOperationHandle<long> result)
    {
        totalSize = result.Result;
        events.NotifySizeDownloaded(result.Result);
    }

    private void OnDependenciesDownloaded(AsyncOperationHandle result)
    {
        events.NotifyDownloadFinished(result.Status == AsyncOperationStatus.Succeeded);
    }

    private void OnException(AsyncOperationHandle handle, Exception exp)
    {
        Debug.LogError("CustomExceptionCaugh !:" + exp.Message);

        if (exp is UnityEngine.ResourceManagement.Exceptions.RemoteProviderException)
        {
            // Remote 관련 에러 발생시
        }
    }

    #endregion

    #region Load

    /// <summary>
    /// key(주소)를 받아 비동기(Async) 로드
    /// </summary>
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (assets.TryGetValue(key, out UnityEngine.Object bundle))
        {
            callback?.Invoke(bundle as T);
            return;
        }

        string loadKey = key;

        var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperation.Completed += op =>
        {
            assets.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadSprites(string key)
    {
        var asyncOperation = Addressables.LoadAssetAsync<SpriteAtlas>(key);
        asyncOperation.Completed += op =>
        {
            spriteAtlas = op.Result;
        };
    }

    /// <summary>
    /// 해당 label을 가진 모든 리소스를 비동기 로드
    /// 완료되면 콜백(key, 현재로드수, 전체로드수) 호출
    /// </summary>
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));

        operation.Completed += op =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (IResourceLocation result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, obj =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (!assets.TryGetValue(key, out UnityEngine.Object bundle)) return null;
        return bundle as T;
    }

    public void Unload<T>(string key) where T : UnityEngine.Object
    {
        if (assets.TryGetValue(key, out UnityEngine.Object bundle))
        {
            Addressables.Release(bundle);
            assets.Remove(key);
        }
        else
        {
            Debug.LogError($"Assets Unload {key}");
        }
    }

    #endregion

    #region Instantiate Prefab

    public GameObject InstantiatePrefab(string key, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"{key}.prefab");
        if (prefab == null)
        {
            Debug.LogError($"Instantiate({key}): Failed to load prefab.");
            return null;
        }

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    #endregion

    #region Blueprint

    public ScriptableObject GetBlueprint(string key)
    {
        ScriptableObject blueprint = Load<ScriptableObject>($"{key}.asset");
        if (blueprint == null)
        {
            Debug.LogError($"Blueprint({key}): Failed to load Blueprint.");
            return null;
        }

        return blueprint;
    }

    #endregion

    #region Text

    public string GetText(string key)
    {
        TextAsset data = Load<TextAsset>($"{key}.json");
        if (data == null)
        {
            Debug.LogError($"Text({key}): Failed to load Text.");
            return null;
        }

        return data.text;
    }

    #endregion

    #region Audio

    public AudioClip GetAudioBGM(string key)
    {
        AudioClip audio = Load<AudioClip>($"{key}.mp3");
        if (audio == null)
        {
            Debug.LogError($"BGM({key}): Failed to load Audio.");
            return null;
        }

        return audio;
    }

    public AudioClip GetAudioSFX(string key)
    {
        AudioClip audio = Load<AudioClip>($"{key}.wav");
        if (audio == null)
        {
            Debug.LogError($"SFX({key}): Failed to load Audio.");
            return null;
        }

        return audio;
    }

    #endregion

    #region Sprite

    public Sprite GetSprite(string key)
    {
        if (spriteAtlas == null)
        {
            Debug.LogError($"Sprite({key}): Failed to load sprite.");
            return null;
        }

        Sprite sprite = spriteAtlas.GetSprite(key);
        return sprite;
    }

    #endregion
}