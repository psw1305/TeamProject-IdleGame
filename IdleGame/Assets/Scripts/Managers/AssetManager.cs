using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetManager
{
    private readonly Dictionary<string, UnityEngine.Object> assets = new();

    #region Load

    public void LoadAllAsync(Action<int, int> callback)
    {
        var asyncOperation = Addressables.LoadAssetsAsync<UnityEngine.Object>("Bundle", null);

        asyncOperation.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (UnityEngine.Object loadedObj in op.Result)
            {
                loadCount++;
                string loadKey = loadedObj.name;
                assets.Add(loadKey, loadedObj);
                callback?.Invoke(loadCount, totalCount);
            }
        };
    }

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (!assets.TryGetValue(key, out UnityEngine.Object asset)) return null;
        return asset as T;
    }

    #endregion

    #region Get Asset

    public GameObject GetPrefab(string key)
    {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"Prefab({key}): Failed to load blueprint.");
            return null;
        }

        return prefab;
    }

    public GameObject InstantiatePrefab(string key, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"Instantiate({key}): Failed to load prefab.");
            return null;
        }

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public ScriptableObject GetBlueprint(string key)
    {
        ScriptableObject blueprint = Load<ScriptableObject>(key);
        if (blueprint == null)
        {
            Debug.LogError($"Get({key}): Failed to load blueprint.");
            return null;
        }

        return blueprint;
    }

    public string GetText(string key)
    {
        TextAsset data = Load<TextAsset>(key);
        if (data == null)
        {
            Debug.LogError($"Get({key}): Failed to load text.");
            return null;
        }

        return data.text;
    }

    public AudioClip GetAudio(string key)
    {
        AudioClip audio = Load<AudioClip>(key);
        if (audio == null)
        {
            Debug.LogError($"Get({key}): Failed to load audio.");
            return null;
        }

        return audio;
    }

    #endregion
}
