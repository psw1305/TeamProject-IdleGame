using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowerDataContainer", menuName = "Blueprints/FollowerDataContainer")]
public class FollowerContainerBlueprint : ScriptableObject
{
    public List<FollowerBlueprint> followerDatas = new();
}

[System.Serializable]
public class FollowerBlueprint
{
    [Header("Follower Info")]
    [SerializeField] private string itemID;
    [SerializeField] private string followerName;
    [SerializeField] private GameObject followerObject;
    [SerializeField] private Sprite sprite;
    [SerializeField] private RuntimeAnimatorController animator;
    [SerializeField] private string rarity;
    
    [Header("Follower Stats")]
    [SerializeField] private float damageCorrection;
    [SerializeField] private float atkSpeed;
    [SerializeField] private float reinforceDamage;
    [SerializeField] private float reinforceEffect;
    [SerializeField] private float retentionEffect;

    public string ItemID => itemID;
    public string FollowerName => followerName;
    public GameObject FollowerObject => followerObject;
    public  Sprite Sprite => sprite;
    public RuntimeAnimatorController Animator => animator;
    public string Rarity => rarity;

    public float DamageCorrection => damageCorrection;
    public float AtkSpeed => atkSpeed;
    public float ReinforceDamage => reinforceDamage;
    public float RetentionEffect => retentionEffect;
    public float ReinforceEffect => reinforceEffect;
}
