using UnityEngine;

public class Utility : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        return obj.GetComponent<T>() ?? obj.AddComponent<T>();
    }

    public static T InstantiateUI<T>(GameObject popupObject, Transform parent = null) where T : Component
    {
        var obj = GameObject.Instantiate(popupObject, parent);
        return GetOrAddComponent<T>(obj);
    }

    public static void DestroyUI(UIBase popup)
    {
        GameObject.Destroy(popup.gameObject);
    }
}
