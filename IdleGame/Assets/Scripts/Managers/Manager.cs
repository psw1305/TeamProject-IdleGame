using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Singleton

    private static Manager instance;
    private static bool initialized;
    public static Manager Instance
    {
        get
        {
            if (!initialized)
            {
                initialized = true;

                GameObject obj = GameObject.Find("@Manager");
                if (obj == null)
                {
                    obj = new() { name = @"Manager" };
                    obj.AddComponent<Manager>();
                    DontDestroyOnLoad(obj);
                    instance = obj.GetComponent<Manager>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region Manage

    private readonly ResourceManager resource = new();
    private readonly GameManager game = new();
    private readonly UIManager ui = new();

    public static ResourceManager Resource => Instance != null ? Instance.resource : null;
    public static GameManager Game => Instance != null ? Instance.game : null;
    public static UIManager UI => Instance != null ? Instance.ui : null;

    #endregion
}
