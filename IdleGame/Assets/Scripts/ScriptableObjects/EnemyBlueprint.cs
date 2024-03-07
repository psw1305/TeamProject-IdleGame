using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBlueprint", menuName = "Blueprints/EnemyBlueprint")]
public class EnemyBlueprint : ScriptableObject
{
    [Header("Enemy Info")]
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private GameObject projectailVFX;
    [SerializeField] private EnemyType enemyType;

    [Header("Enemy Stats")]
    [SerializeField] private long damage;
    [SerializeField] private long hp;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private long rewards;
    [SerializeField] private float moveSpeed;

    public string EnemyName => enemyName;
    public Sprite EnemySprite => enemySprite;
    public GameObject ProjectailVFX => projectailVFX;

    public EnemyType EnemyType => enemyType;
    public long Damage => damage;
    public long HP => hp;
    public float Range => range;
    public float AttackSpeed => attackSpeed;
    public long Rewards => rewards;
    public float MoveSpeed => moveSpeed;
}
