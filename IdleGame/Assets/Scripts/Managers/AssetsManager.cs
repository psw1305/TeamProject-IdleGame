using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetsManager
{
    #region Fields

    private readonly string localURL = $"{Application.dataPath}/AssetBundles/";
    private readonly string serverURL = "https://firebasestorage.googleapis.com/v0/b/portfolio-idlegame.appspot.com/o/prefab?alt=media&token=cca8adbe-d3c4-4f90-9068-d44193ff584e";
    private readonly Dictionary<string, Object> bundles = new();

    #endregion

    #region Download Files

    /// <summary>
    /// [Editor] 에셋번들 데이터 파일 로컬 다운로드
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownloadLocalFiles()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(localURL, "bundles"));
        
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
                Debug.Log(assetName);
            }

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
        using UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(serverURL);

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
                bundles.Add(assetName, (Object)asset);
            }

            // 에셋 번들 해제
            bundle.Unload(false);
        }

        yield return null;
    }

    #endregion

    #region Instantiate Prefab

    public GameObject InstantiateModel(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/model/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object model)) return null;
        
        Object obj = Object.Instantiate(model, parent);
        obj.name = model.name;
        return (GameObject)obj;
    }

    public GameObject InstantiateFollower(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/follower/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object follower)) return null;

        Object obj = Object.Instantiate(follower, parent);
        obj.name = follower.name;
        return (GameObject)obj;
    }

    public GameObject InstantiateProjectile(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/projectile/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object projectile)) return null;

        Object obj = Object.Instantiate(projectile, parent);
        obj.name = projectile.name;
        return (GameObject)obj;
    }

    public GameObject InstantiateSkill(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/skill/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object skill)) return null;

        Object obj = Object.Instantiate(skill, parent);
        obj.name = skill.name;
        return (GameObject)obj;
    }

    public GameObject InstantiateUI(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/ui/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object ui)) return null;

        Object obj = Object.Instantiate(ui, parent);
        obj.name = ui.name;
        return (GameObject)obj;
    }

    public GameObject InstantiateUIElement(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/ui/elements/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object element)) return null;

        Object obj = Object.Instantiate(element, parent);
        obj.name = element.name;
        return (GameObject)obj;
    }

    public GameObject InstantiateUIPopup(string assetName, Transform parent = null)
    {
        string key = $"assets/@resources/prefabs/ui/popup/{assetName.ToLower()}.prefab";
        if (!bundles.TryGetValue(key, out Object popup)) return null;

        Object obj = Object.Instantiate(popup, parent);
        obj.name = popup.name;
        return (GameObject)obj;
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

    #region Blueprints

    public ScriptableObject GetBlueprintFollower(string assetName)
    {
        string key = $"assets/@resources/blueprints/follower/{assetName.ToLower()}.asset";
        if (!bundles.TryGetValue(key, out Object blueprint)) return null;

        return (ScriptableObject)blueprint;
    }

    public ScriptableObject GetBlueprintSkill(string assetName)
    {
        string key = $"assets/@resources/blueprints/skill/{assetName.ToLower()}.asset";
        if (!bundles.TryGetValue(key, out Object blueprint)) return null;

        return (ScriptableObject)blueprint;
    }

    public ScriptableObject GetBlueprintStage(string assetName)
    {
        string key = $"assets/@resources/blueprints/stage/{assetName.ToLower()}.asset";
        if (!bundles.TryGetValue(key, out Object blueprint)) return null;

        return (ScriptableObject)blueprint;
    }

    public ScriptableObject GetBlueprintEnemy(string assetName)
    {
        string key = $"assets/@resources/blueprints/enemy/{assetName.ToLower()}.asset";
        if (!bundles.TryGetValue(key, out Object blueprint)) return null;

        return (ScriptableObject)blueprint;
    }

    public ScriptableObject GetBlueprintSummon(string assetName)
    {
        string key = $"assets/@resources/blueprints/summon/{assetName.ToLower()}.asset";
        if (!bundles.TryGetValue(key, out Object blueprint)) return null;

        return (ScriptableObject)blueprint;
    }

    #endregion

    #region Texts

    public string GetTextItem(string assetName)
    {
        string key = $"assets/@resources/texts/item/{assetName.ToLower()}.json";
        if (!bundles.TryGetValue(key, out Object itemText)) return null;

        var textAsset = itemText as TextAsset;
        return textAsset.text;
    }

    public string GetTextSummon(string assetName)
    {
        string key = $"assets/@resources/texts/summon/{assetName.ToLower()}.json";
        if (!bundles.TryGetValue(key, out Object summonText)) return null;

        var textAsset = summonText as TextAsset;
        return textAsset.text;
    }

    public string GetTextUser(string assetName)
    {
        string key = $"assets/@resources/texts/user/{assetName.ToLower()}.json";
        if (!bundles.TryGetValue(key, out Object userText)) return null;

        var textAsset = userText as TextAsset;
        return textAsset.text;
    }

    #endregion

    #region Audios

    public AudioClip GetAudioBGM(string assetName)
    {
        string key = $"assets/@resources/audios/bgm/{assetName.ToLower()}.mp3";
        if (!bundles.TryGetValue(key, out Object audio)) return null;
        return (AudioClip)audio;
    }

    public AudioClip GetAudioSFX(string assetName)
    {
        string key = $"assets/@resources/audios/sfx/{assetName.ToLower()}.wav";
        if (!bundles.TryGetValue(key, out Object audio)) return null;
        return (AudioClip)audio;
    }

    #endregion
}
