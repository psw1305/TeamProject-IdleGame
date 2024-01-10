using UnityEngine;

public class Utility : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        return obj.GetComponent<T>() ?? obj.AddComponent<T>();
    }
}
