using DG.Tweening.Plugins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager
{
    public IObjectPool<GameObject> pool { get; private set; }

    private string objectName;

    public void Initialize()
    {
        pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);

        //for(int i=0; i< 10; i++)
        //{
        //    ProjectileHandlerBase project = CreateProjectile().GetComponent<ProjectileHandlerBase>();
        //    project.Poolable.Release(project.gameObject);
        //}
    }

    private GameObject CreateProjectile()
    {
        //GameObject projectile = Manager.Resource.InstantiatePrefab("FollowerProjectileFrame");
        //projectile.GetComponent<ProjectileHandlerBase>().SetManagedPool(pool);
        //return projectile;

        GameObject poolGo = Manager.Resource.InstantiatePrefab(objectName);
        poolGo.GetComponent<ObjectPoolable>().SetManagedPool(pool);
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

        return pool.Get();
    }
}
