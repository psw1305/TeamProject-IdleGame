using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager
{
    private string objectName;

    private string[] _poolStringArray = new string[5] { "PlayerProjectileFrame", "EnemyProjectileFrame", "FollowerProjectileFrame", "Canvas_FloatingDamage", "EnemyFrame" };

    private Dictionary<string, IObjectPool<GameObject>> poolDict = new Dictionary<string, IObjectPool<GameObject>>();

    public void Initialize()
    {
        for (int i = 0; i < _poolStringArray.Length; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);

            poolDict.Add(_poolStringArray[i], pool);

            for (int j = 0; j < 50; j++)
            {
                objectName = _poolStringArray[i];
                ObjectPoolable poolGo = CreateProjectile().GetComponent<ObjectPoolable>();
                poolGo.Poolable.Release(poolGo.gameObject);
            }
        }
    }

    private GameObject CreateProjectile()
    {
        GameObject poolGo = Manager.Resource.InstantiatePrefab(objectName);
        poolGo.GetComponent<ObjectPoolable>().SetManagedPool(poolDict[objectName]);
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
        objectName = goName;

        return poolDict[goName].Get();
    }
}
