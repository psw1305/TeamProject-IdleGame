using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Blueprints/Enemy")]
public class EnemyBlueprint : ScriptableObject
{
    [Header("Enemy Info")]
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite enemySprite;

    [Header("Enemy Stats")]
    [SerializeField] private int damage;
    [SerializeField] private int hp;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int rewards;

    public string EnemyName => enemyName;
    public Sprite EnemySprite => enemySprite;
    public int Damage => damage;
    public int HP => hp;
    public float Range => range;
    public float AttackSpeed => attackSpeed;
    public int Rewards => rewards;
}
