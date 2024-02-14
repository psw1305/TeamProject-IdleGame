using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadEvents
{
    // 어드레서블 시스템 초기화
    public event Action SystemInitializedListner;
    public void NotifyInitialized() => SystemInitializedListner?.Invoke();

    // Catalog 업데이트 완료
    public event Action CatalogUpdatedListenr;
    public void NotifyCatalogUpdated() => CatalogUpdatedListenr?.Invoke();

    // Size 다운로드 완료
    public event Action<long> SizeDownloadedListner;
    public void NotifySizeDownloaded(long size) => SizeDownloadedListner?.Invoke(size);

    // 다운로드 진행
    public event Action<DownloadProgressStatus> DownloadProgressListner;
    public void NotifyDownloadProgress(DownloadProgressStatus status) => DownloadProgressListner?.Invoke(status);

    // Bundle 다운로드 완료
    public event Action<bool> DownloadFinished;
    public void NotifyDownloadFinished(bool isSuccess) => DownloadFinished?.Invoke(isSuccess);
}
