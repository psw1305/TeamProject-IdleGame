using System.Collections;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    [SerializeField] private DownloadPopup downloadPopup;

    private bool initialized = false;

    protected virtual bool Initialize()
    {
        if (initialized) return false;

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
        yield return StartCoroutine(downloadPopup.StartDownload());

        //yield return StartCoroutine(Manager.Data.Load());
    }

    private void OnApplicationQuit()
    {
        Manager.Data.Save();
    }

    #endregion
}
