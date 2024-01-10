using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private Dictionary<string, GameObject> prefabs = new();
    private Dictionary<string, Sprite> sprites = new();

    /// <summary>
    /// Resources 폴더 안 아이템 불러오기
    /// </summary>
    public void Initialize()
    {
        LoadPrefabs("Prefabs", prefabs);
        LoadSprites("Sprites", sprites);
    }

    #region Prefab

    /// <summary>
    /// 지정된 경로 안에 모든 프리팹들 로드
    /// </summary>
    /// <param name="path">폴더 경로</param>
    /// <param name="prefabs">로드할 프리팹 값</param>
    private void LoadPrefabs(string path, Dictionary<string, GameObject> prefabs)
    {
        GameObject[] objs = Resources.LoadAll<GameObject>(path);
        foreach (GameObject obj in objs)
        {
            prefabs[obj.name] = obj;
        }
    }

    /// <summary>
    /// string key를 기반으로 오브젝트 가져오기
    /// </summary>
    /// <param name="key">프리팹 이름</param>
    /// <returns></returns>
    public GameObject InstantiatePrefab(string key, Transform parent = null)
    {
        if (!prefabs.TryGetValue(key, out GameObject prefab)) return null;
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    // 해당 오브젝트를 파괴한다.
    public void Destroy(GameObject obj)
    {
        if (obj == null) return;
        UnityEngine.Object.Destroy(obj);
    }

    #endregion

    #region Sprite

    /// <summary>
    /// 지정된 경로 안에 모든 스프라이트 로드
    /// </summary>
    /// <param name="path">폴더 경로</param>
    /// <param name="sprites">로드할 스프라이트 값</param>
    private void LoadSprites(string path, Dictionary<string, Sprite> sprites)
    {
        Sprite[] objs = Resources.LoadAll<Sprite>(path);
        foreach (Sprite obj in objs)
        {
            sprites[obj.name] = obj;
        }
    }

    public Sprite GetSprite(string spriteName)
    {
        if (!sprites.TryGetValue(spriteName, out Sprite sprite)) return null;
        return sprite;
    }

    #endregion
}
