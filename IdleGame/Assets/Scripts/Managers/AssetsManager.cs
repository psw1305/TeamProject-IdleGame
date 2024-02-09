using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetsManager
{
    #region Fields

    private readonly string materialURL = "https://firebasestorage.googleapis.com/v0/b/portfolio-idlegame.appspot.com/o/materials?alt=media&token=50263c7e-4690-4f7e-a1d7-cf00b054ae39";
    private readonly string bundleURL = "https://firebasestorage.googleapis.com/v0/b/portfolio-idlegame.appspot.com/o/prefab?alt=media&token=e17d97e8-b0be-495e-be1c-7aa7df7f883b";
    private Dictionary<string, GameObject> prefabs = new();

    #endregion

    public IEnumerator DownloadFiles()
    {
        using UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("에셋 번들 로딩 중 에러 발생: " + www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            // 파일 가져오기 및 딕셔너리에 저장
            foreach (string assetName in bundle.GetAllAssetNames())
            {
                Object asset = bundle.LoadAsset(assetName);
                prefabs.Add(assetName, (GameObject)asset);
            }

            // 에셋 번들 해제
            bundle.Unload(false);
        }

        yield return null;

        Debug.Log("File Download Complete");
    }

    #region Instantiate Prefab

    public GameObject InstantiateModel(string modelName, Transform parent = null)
    {
        string key = $"assets/bundleassets/prefabs/model/{modelName.ToLower()}.prefab";
        if (!prefabs.TryGetValue(key, out GameObject prefab)) return null;
        
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public GameObject InstantiateFollower(string followerName, Transform parent = null)
    {
        string key = $"assets/bundleassets/prefabs/follower/{followerName.ToLower()}.prefab";
        if (!prefabs.TryGetValue(key, out GameObject prefab)) return null;

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public GameObject InstantiateUI(string uiName, Transform parent = null)
    {
        string key = $"assets/bundleassets/prefabs/ui/{uiName.ToLower()}.prefab";
        if (!prefabs.TryGetValue(key, out GameObject prefab)) return null;

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public GameObject InstantiateUIElement(string uiName, Transform parent = null)
    {
        string key = $"assets/bundleassets/prefabs/ui/elements/{uiName.ToLower()}.prefab";
        if (!prefabs.TryGetValue(key, out GameObject prefab)) return null;

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public GameObject InstantiateUIPopup(string uiName, Transform parent = null)
    {
        string key = $"assets/bundleassets/prefabs/ui/popup/{uiName.ToLower()}.prefab";
        if (!prefabs.TryGetValue(key, out GameObject prefab)) return null;

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    #endregion
}
