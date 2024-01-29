using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolable : MonoBehaviour
{
    public IObjectPool<GameObject> Poolable { get; private set; }

    public void SetManagedPool(IObjectPool<GameObject> pool)
    {
        Poolable = pool;
    }

    public void ReleaseObject()
    {
        Poolable.Release(gameObject);
    }
}
