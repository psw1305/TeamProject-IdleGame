using DG.Tweening.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager
{
    //public IObjectPool<GameObject> pool { get; private set; }

    private string objectName;

    private string[] _poolStringArray = new string[3] { "PlayerProjectileFrame", "EnemyProjectileFrame", "FollowerProjectileFrame"};

    private Dictionary<string, IObjectPool<GameObject>> poolDict = new Dictionary<string, IObjectPool<GameObject>>();

    public void Initialize()
    {
        //pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);

        for (int i = 0; i < _poolStringArray.Length; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);

            poolDict.Add(_poolStringArray[i], pool);
        }
    }

    private GameObject CreateProjectile()
    {
        //GameObject projectile = Manager.Resource.InstantiatePrefab("FollowerProjectileFrame");
        //projectile.GetComponent<ProjectileHandlerBase>().SetManagedPool(pool);
        //return projectile;

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
