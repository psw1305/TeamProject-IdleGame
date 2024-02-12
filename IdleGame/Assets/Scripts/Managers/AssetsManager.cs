using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetsManager
{
    #region Fields

    private readonly string localPath = $"{Application.dataPath}/AssetBundles/";
    private readonly string materialURL = "https://firebasestorage.googleapis.com/v0/b/portfolio-idlegame.appspot.com/o/materials?alt=media&token=b160897e-2554-47cd-93d0-9d54b77222cc";
    private readonly string bundleURL = "https://firebasestorage.googleapis.com/v0/b/portfolio-idlegame.appspot.com/o/bundles?alt=media&token=b3ce679c-48c5-475f-a6a8-1b001056c5f0";
    private readonly Dictionary<string, Object> bundles = new();

    #endregion

    #region Download Files

    /// <summary>
    /// [Editor] 에셋번들 데이터 파일 로컬 다운로드
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownloadLocalFiles()
    {
        var materials = AssetBundle.LoadFromFile(Path.Combine(localPath, "materials"));
        var bundle = AssetBundle.LoadFromFile(Path.Combine(localPath, "bundles"));
        
        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
        else
        {
            // 파일 가져오기 및 딕셔너리에 저장
            foreach (string assetName in bundle.GetAllAssetNames())
            {
                Object asset = bundle.LoadAsset(assetName);
                bundles.Add(assetName, asset);
            }

            materials.Unload(false);
            bundle.Unload(false);
        }

        yield return null;
    }

    /// <summary>
    /// [Android] 에셋번들 데이터 파일 서버 다운로드
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownloadServerFiles()
    {
        using UnityWebRequest wwwMaterial = UnityWebRequestAssetBundle.GetAssetBundle(materialURL);
        yield return wwwMaterial.SendWebRequest();

        using UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("에셋 번들 로딩 중 에러 발생: " + www.error);
        }
        else
        {
            AssetBundle material = DownloadHandlerAssetBundle.GetContent(wwwMaterial);
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            // 파일 가져오기 및 딕셔너리에 저장
            foreach (string assetName in bundle.GetAllAssetNames())
            {
                Object asset = bundle.LoadAsset(assetName);
                bundles.Add(assetName, (Object)asset);
            }

            material.Unload(false);
            bundle.Unload(false);
        }

        yield return null;
    }

    #endregion

    #region Sprite

    public Sprite GetSprite(string assetName)
    {
        string key = $"assets/@resources/sprites/follower/{assetName.ToLower()}.png";
        if (!bundles.TryGetValue(key, out Object sprite)) return null;

        return (Sprite)sprite;
    }

    public Sprite GetSpriteFollower(string assetName)
    {
        string key = $"assets/@resources/sprites/follower/{assetName.ToLower()}.asset";
        if (!bundles.TryGetValue(key, out Object sprite)) return null;

        return (Sprite)sprite;
    }

    #endregion
}
