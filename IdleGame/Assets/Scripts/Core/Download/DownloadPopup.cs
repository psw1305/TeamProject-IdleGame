using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DownloadPopup : MonoBehaviour
{
    public enum State
    {
        None = 0,
        CalculatingSize,
        NothingToDownload,
        AskingDownload,
        Downloading,
        DownloadFinished,
    }

    [Serializable]
    public class Root
    {
        public State state;
        public GameObject root;
    }

    #region Serialize Fields

    [SerializeField] private List<Root> roots;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtContent;
    [SerializeField] private TextMeshProUGUI txtDownloadingBarStatus;
    [SerializeField] private Slider downloadProgressBar;
    [SerializeField] private DownloadController downloader;

    #endregion

    #region Fields

    private DownloadProgressStatus progressInfo;
    private SizeUnits sizeUnit;
    private long curDownloadedSizeInUnit;
    private long totalSizeInUnit;

    #endregion

    public State CurrentState { get; private set; } = State.None;

    #region Unity Flow

    private IEnumerator Start()
    {
        SetState(State.CalculatingSize, true);

        yield return downloader.StartDownloadRoutine((events) => 
        {
            events.SystemInitializedListener += OnInitialized;
            events.CatalogUpdatedListener += OnCatalogUpdated;
            events.SizeDownloadedListener += OnSizeDownloaded;
            events.DownloadProgressListener += OnDownloadProgress;
            events.DownloadFinished += OnDownloadFinished;
        });
    }

    private void SetState(State newState, bool updateUI) 
    {
        var prevRoot = roots.Find(t => t.state == CurrentState);
        var newRoot = roots.Find(t => t.state == newState);

        CurrentState = newState;

        if (prevRoot != null)
        {
            prevRoot.root.gameObject.SetActive(false);
        }

        if (newRoot != null)
        {
            newRoot.root.gameObject.SetActive(true);
        }

        if (updateUI)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (CurrentState == State.CalculatingSize)
        {
            txtTitle.text = "알림";
            txtContent.text = "다운로드 정보를 가져오고 있습니다. 잠시만 기다려주세요.";
        }
        else if (CurrentState == State.NothingToDownload)
        {
            txtTitle.text = "완료";
            txtContent.text = "다운로드 받을 데이터가 없습니다.";
        }
        else if (CurrentState == State.AskingDownload)
        {
            txtTitle.text = "주의";
            txtContent.text = $"다운로드를 받으시겠습니까? 데이터가 많이 사용될 수 있습니다. <color=green>({totalSizeInUnit}{sizeUnit})</color>";
        }
        else if (CurrentState == State.Downloading)
        {
            txtTitle.text = "다운로드 중";
            txtContent.text = $"다운로드 중입니다. 잠시만 기다려주세요. {(progressInfo.totalProgress * 100).ToString("0.00")}% 완료";

            downloadProgressBar.value = progressInfo.totalProgress;
            txtDownloadingBarStatus.text = $"{curDownloadedSizeInUnit}/{totalSizeInUnit}{sizeUnit}";
        }
        else if (CurrentState == State.DownloadFinished)
        {
            txtTitle.text = "완료";
            txtContent.text = "다운로드가 완료되었습니다. 게임을 진행하시겠습니까?";
        }
    }

    #endregion

    #region Button Events

    public void OnClickStartDownload()
    {
        SetState(State.Downloading, true);
        downloader.GoNext();
    }

    public void OnClickCancleBtn()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

    public void OnClickEnterGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    #endregion

    #region Download Events

    private void OnInitialized()
    {
        downloader.GoNext();
    }

    private void OnCatalogUpdated()
    {
        downloader.GoNext();
    }

    private void OnSizeDownloaded(long size)
    {
        if (size == 0)
        {
            SetState(State.NothingToDownload, true);
        }
        else
        {
            sizeUnit = Utilities.GetProperByteUnit(size);
            totalSizeInUnit = Utilities.ConvertByteByUnit(size, sizeUnit);
            SetState(State.AskingDownload, true);
        }
    }

    private void OnDownloadProgress(DownloadProgressStatus newInfo)
    {
        bool changed = progressInfo.downloadedBytes != newInfo.downloadedBytes;

        progressInfo = newInfo;

        if (changed)
        {
            UpdateUI();
            curDownloadedSizeInUnit = Utilities.ConvertByteByUnit(newInfo.downloadedBytes, sizeUnit);
        }
    }

    private void OnDownloadFinished(bool isSuccess)
    {
        SetState(State.DownloadFinished, true);
        downloader.GoNext();
    }

    #endregion
}
