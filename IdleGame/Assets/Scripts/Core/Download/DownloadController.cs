using System;
using System.Collections;
using UnityEngine;

public class DownloadController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Initialize,
        UpdateCatalog,
        DownloadSize,
        DownloadDependencies,
        Downloading,
        Finished
    }

    [SerializeField] private string downloadURL;

    private AddressableDownloader downloader;
    private Action<DownloadEvents> onEventObtained;

    public State CurrentState { get; private set; } = State.Idle;
    public State LastValidState { get; private set; } = State.Idle;

    public IEnumerator StartDownloadRoutine(Action<DownloadEvents> onEventObtained)
    {
        downloader = new AddressableDownloader();
        this.onEventObtained = onEventObtained;

        LastValidState = CurrentState = State.Initialize;

        while(CurrentState != State.Finished)
        {
            OnExecute();
            yield return null;
        }
    }

    public void GoNext()
    {
        if (LastValidState == State.Initialize)
        {
            CurrentState = State.UpdateCatalog;
        }
        else if (LastValidState == State.UpdateCatalog)
        {
            CurrentState = State.DownloadSize;
        }
        else if (LastValidState == State.DownloadSize)
        {
            CurrentState = State.DownloadDependencies;
        }
        else if (LastValidState == State.Downloading || LastValidState == State.DownloadDependencies)
        {
            CurrentState = State.Finished;
        }

        LastValidState = CurrentState;
    }

    private void OnExecute()
    {
        if (CurrentState == State.Idle)
        {
            return;
        }

        if (CurrentState == State.Initialize)
        {
            var events = downloader.InitializeSystem("Bundle", downloadURL);
            this.onEventObtained?.Invoke(events);

            CurrentState = State.Idle;
        }
        else if (CurrentState == State.UpdateCatalog)
        {
            downloader.UpdateCatalog();
            CurrentState = State.Idle;
        }
        else if (CurrentState == State.DownloadSize)
        {
            downloader.DownloadSize();
            CurrentState = State.Idle;
        }
        else if (CurrentState == State.DownloadDependencies)
        {
            downloader.StartDownload();
            CurrentState = State.Downloading;
        }
        else if (CurrentState == State.Downloading)
        {
            downloader.Update();
        }
    }
}
