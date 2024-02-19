using UnityEngine;

public class BaseScene : MonoBehaviour
{
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

    private void Start()
    {
        AddressableLoad();
    }

    private void OnApplicationQuit()
    {
        Manager.Data.Save();
    }

    #endregion

    private void AddressableLoad()
    {
        Manager.Asset.LoadAllAsync<Object>("Bundle", (key, count, totalCount) =>
        {
            if (count >= totalCount)
            {
                Manager.Data.Load();
                Manager.Game.Initialize();
            }
        });
    }
}
