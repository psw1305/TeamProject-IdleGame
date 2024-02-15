using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataContainer", menuName = "Blueprints/ItemDataContainer")]
public class ItemContainerBlueprint : ScriptableObject
{
    public List<ItemBlueprint> itemBlueprints = new();
}

[System.Serializable]
public class ItemBlueprint
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [SerializeField] private string rarity;
    [SerializeField] private string statType;
    [SerializeField] private float equipStat;
    [SerializeField] private float reinforceEquip;
    [SerializeField] private float retentionEffect;
    [SerializeField] private float reinforceEffect;


    public Sprite Sprite => _sprite;
    public string ItemID => itemID;
    public string ItemName => itemName;
    public string Rarity => rarity;
    public string StatType => statType;
    public float EquipStat => equipStat;
    public float  ReinforceEquip => reinforceEquip;
    public float RetentionEffect => retentionEffect;
    public float ReinforceEffect => reinforceEffect;
}
