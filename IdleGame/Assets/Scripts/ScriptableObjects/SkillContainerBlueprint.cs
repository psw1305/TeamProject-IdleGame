using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataContainer", menuName = "Blueprints/SkillDataContainer")]
public class SkillContainerBlueprint : ScriptableObject
{
    public List<SkillBlueprint> skillDatas = new();

    public Sprite FindSprite(string id)
    {
        return skillDatas.FirstOrDefault(item => item.ItemID == id)?.Sprite;
    }
}

[System.Serializable]
public class SkillBlueprint
{
    [Header("Skill Info")]
    [SerializeField] private string itemID;
    [SerializeField] private string skillName;
    [SerializeField] private ItemTier rarity;
    [SerializeField] private string description;
    [SerializeField] private float skillDamage;
    [SerializeField] private float reinforceDamage;
    [SerializeField] private float retentionEffect;
    [SerializeField] private float reinforceEffect;

    [Header("Skill Resource")]
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject skillObject;


    public Sprite Sprite => _sprite;
    public GameObject SkillObject => skillObject;
    public string ItemID => itemID;
    public string SkillName => skillName;
    public ItemTier Rarity => rarity;
    public string Description => description;
    public float SkillDamage => skillDamage;
    public float ReinforceDamage => reinforceDamage;
    public float RetentionEffect => retentionEffect;
    public float ReinforceEffect => reinforceEffect;
}