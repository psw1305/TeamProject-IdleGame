using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Blueprints/Enemy")]
public class EnemyBlueprint : ScriptableObject
{
    [Header("Enemy Info")]
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private GameObject projectailVFX;

    [Header("Enemy Stats")]
    [SerializeField] private int damage;
    [SerializeField] private int hp;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int rewards;
    [SerializeField] private float moveSpeed;

    public string EnemyName => enemyName;
    public Sprite EnemySprite => enemySprite;
    public GameObject ProjectailVFX => projectailVFX;
    public int Damage => damage;
    public int HP => hp;
    public float Range => range;
    public float AttackSpeed => attackSpeed;
    public int Rewards => rewards;
    public float MoveSpeed => moveSpeed;
}
