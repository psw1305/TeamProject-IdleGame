using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataContainer", menuName = "Blueprints/ItemDataContainer")]
public class ItemContainerBlueprint : ScriptableObject
{
    public List<ItemBlueprint> itemDatas = new();

    public Sprite FindSprite(string id)
    {
        return itemDatas.FirstOrDefault(item => item.ItemID == id)?.Sprite;
    }
}

[System.Serializable]
public class ItemBlueprint
{
    [Header("Equip Info")]
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [SerializeField] private ItemTier rarity;
    [SerializeField] private StatType statType;
    [SerializeField] private float equipStat;
    [SerializeField] private float reinforceEquip;
    [SerializeField] private float retentionEffect;
    [SerializeField] private float reinforceEffect;

    [Header("Equip Resource")]
    [SerializeField] private Sprite _sprite;

    public Sprite Sprite => _sprite;
    public string ItemID => itemID;
    public string ItemName => itemName;
    public ItemTier Rarity => rarity;
    public StatType StatType => statType;
    public float EquipStat => equipStat;
    public float  ReinforceEquip => reinforceEquip;
    public float RetentionEffect => retentionEffect;
    public float ReinforceEffect => reinforceEffect;
}
