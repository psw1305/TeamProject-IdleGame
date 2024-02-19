using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableDownloader
{
    #region Fields

    public static string DownloadURL;
    private DownloadEvents events;
    private string labelToDownload;
    private long totalSize;
    private AsyncOperationHandle downloadHandle;

    #endregion

    #region Work Flow

    public DownloadEvents InitializeSystem(string label, string downloadURL)
    {
        events = new DownloadEvents();

        Addressables.InitializeAsync().Completed += OnInitialized;

        labelToDownload = label;
        DownloadURL = downloadURL;

        ResourceManager.ExceptionHandler += OnException;

        return events;
    }

    public void UpdateCatalog()
    {
        Addressables.CheckForCatalogUpdates().Completed += (result) =>
        {
            var catalogToUpdate = result.Result;
            if (catalogToUpdate.Count > 0)
            {
                Addressables.UpdateCatalogs(catalogToUpdate).Completed += OnCatalogUpdated;
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
        downloadHandle = Addressables.DownloadDependenciesAsync(labelToDownload, true);
        downloadHandle.Completed += OnDependenciesDownloaded;
    }

    public void Update()
    {
        if (downloadHandle.IsValid()
            && downloadHandle.IsDone == false
            && downloadHandle.Status != AsyncOperationStatus.Failed)
        {
            var status = downloadHandle.GetDownloadStatus();

            long curDownloadedSize = status.DownloadedBytes;
            long remainedSize = totalSize - curDownloadedSize;

            events.NotifyDownloadProgress(new DownloadProgressStatus(
                    status.DownloadedBytes,
                    totalSize,
                    remainedSize,
                    status.Percent));
        }
    }

    #endregion

    #region Events

    private void OnInitialized(AsyncOperationHandle<IResourceLocator> result)
    {
        events.NotifyInitialized();
    }

    void OnCatalogUpdated(AsyncOperationHandle<List<IResourceLocator>> result)
    {
        events.NotifyCatalogUpdated();
    }

    void OnSizeDownloaded(AsyncOperationHandle<long> result)
    {
        totalSize = result.Result;
        events.NotifySizeDownloaded(result.Result);
    }

    void OnDependenciesDownloaded(AsyncOperationHandle result)
    {
        events.NotifyDownloadFinished(result.Status == AsyncOperationStatus.Succeeded);
    }

    void OnException(AsyncOperationHandle handle, Exception exception)
    {
        Debug.LogError(exception.Message);
    }

    #endregion
}
