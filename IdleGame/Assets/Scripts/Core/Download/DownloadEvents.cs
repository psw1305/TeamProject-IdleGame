using System;

public class DownloadEvents
{
    // 어드레서블 시스템 초기화
    public event Action SystemInitializedListener;
    public void NotifyInitialized() => SystemInitializedListener?.Invoke();

    // Catalog 업데이트 완료
    public event Action CatalogUpdatedListener;
    public void NotifyCatalogUpdated() => CatalogUpdatedListener?.Invoke();

    // Size 다운로드 완료
    public event Action<long> SizeDownloadedListener;
    public void NotifySizeDownloaded(long size) => SizeDownloadedListener?.Invoke(size);

    // 다운로드 진행
    public event Action<DownloadProgressStatus> DownloadProgressListener;
    public void NotifyDownloadProgress(DownloadProgressStatus status) => DownloadProgressListener?.Invoke(status);

    // Bundle 다운로드 완료
    public event Action<bool> DownloadFinished;
    public void NotifyDownloadFinished(bool isSuccess) => DownloadFinished?.Invoke(isSuccess);
}
