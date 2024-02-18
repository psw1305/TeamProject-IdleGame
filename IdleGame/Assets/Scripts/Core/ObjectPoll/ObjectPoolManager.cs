using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager
{
    #region Field

    private class ObjectInfo
    {
        public string ObjectName;
        public int Size;

        public ObjectInfo(string name, int size)
        {
            ObjectName = name;
            Size = size;
        }
    }

    private ObjectInfo[] _poolList = new ObjectInfo[] {
        new ObjectInfo("PlayerProjectileFrame", 20),
        new ObjectInfo("EnemyProjectileFrame", 20),
        new ObjectInfo("FollowerProjectileFrame", 20),
        new ObjectInfo("Canvas_FloatingDamage", 20),
        new ObjectInfo("EnemyFrame", 10)
    };

    private string _objectName;
    
    private Dictionary<string, IObjectPool<GameObject>> _poolDict = new Dictionary<string, IObjectPool<GameObject>>();
    
    #endregion

    #region Init

    public void Initialize()
    {
        for (int i = 0; i < _poolList.Length; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile,
                OnReleaseProjectile, OnDestroyProjectile, maxSize: _poolList[i].Size);

            _poolDict.Add(_poolList[i].ObjectName, pool);

            for (int j = 0; j < _poolList[i].Size; j++)
            {
                _objectName = _poolList[i].ObjectName;
                ObjectPoolable poolGo = CreateProjectile().GetComponent<ObjectPoolable>();
                poolGo.Poolable.Release(poolGo.gameObject);
            }
        }
    }

    #endregion

    #region PoolMethod

    private GameObject CreateProjectile()
    {
        GameObject poolGo = Manager.Asset.InstantiatePrefab(_objectName);
        poolGo.GetComponent<ObjectPoolable>().SetManagedPool(_poolDict[_objectName]);
        return poolGo;
    }

    private void OnGetProjectile(GameObject projectile)
    {
        projectile.SetActive(true);
    }

    private void OnReleaseProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
    }

    private void OnDestroyProjectile(GameObject projectile)
    {
        GameObject.Destroy(projectile);
    }

    public GameObject GetGo(string goName)
    {
        _objectName = goName;

        return _poolDict[goName].Get();
    }

    #endregion
}
