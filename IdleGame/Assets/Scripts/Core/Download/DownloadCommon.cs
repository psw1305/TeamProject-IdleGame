public struct DownloadProgressStatus
{
    public long downloadedBytes;
    public long totalBytes;
    public long remainedBytes;
    public float totalProgress;

    public DownloadProgressStatus(long downloadedBytes, long totalBytes, long remainedBytes, float totalProgress)
    {
        this.downloadedBytes = downloadedBytes;
        this.totalBytes = totalBytes;
        this.remainedBytes = remainedBytes;
        this.totalProgress = totalProgress;
    }
}
