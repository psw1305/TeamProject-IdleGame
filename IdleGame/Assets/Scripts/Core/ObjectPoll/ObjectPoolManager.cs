using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager
{
    public IObjectPool<GameObject> Pool { get; private set; }

    // public Init()으로 변경

    public void Initialize()
    {
        Pool = new ObjectPool<GameObject>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 20);
    }

    private GameObject CreateProjectile()
    {
        GameObject projectile = Manager.Resource.InstantiatePrefab("FollowerProjectileFrame");
        projectile.GetComponent<ProjectileHandlerBase>().SetManagedPool(this.Pool);
        return projectile;
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
}
