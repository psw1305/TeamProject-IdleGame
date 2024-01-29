using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    public UIScene UI { get; protected set; }
    private bool initialized = false;

    private void Start()
    {
        Manager.ObjectPool.Initialize();
        Manager.Resource.Initialize();
        Manager.Game.Initialize();

        Initialize();
    }

    protected virtual bool Initialize()
    {
        if (initialized) return false;

        Object obj = GameObject.FindObjectOfType<EventSystem>();
        if (obj == null) Manager.Resource.InstantiatePrefab("EventSystem").name = "@EventSystem";

        initialized = true;
        return true;
    }
}
