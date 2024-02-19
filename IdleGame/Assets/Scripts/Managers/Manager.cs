using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Singleton

    private static Manager instance;
    private static bool initialized;
    public static Manager Instance
    {
        get
        {
            if (!initialized)
            {
                initialized = true;

                GameObject obj = GameObject.Find("@Manager");
                if (obj == null)
                {
                    obj = new() { name = "@Manager" };
                    obj.AddComponent<Manager>();
                    DontDestroyOnLoad(obj);
                    instance = obj.GetComponent<Manager>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region Manage

    private readonly AssetManager asset = new();
    private readonly SessionManager session = new();
    private readonly DataManager data = new();
    private readonly GameManager game = new();
    private readonly UIManager ui = new();
    private readonly StageManager stage = new();
    private readonly InventoryManager inventory = new();
    private readonly QuestManager quest = new();
    private readonly SummonManager summon = new();
    private readonly NotificateManager notificate = new();
    private readonly ObjectPoolManager objectPool = new();
    private readonly SkillDataManager skillData = new();
    private readonly FollowerDataManager followerData = new();
    private readonly RankingManager ranking = new();

    public static AssetManager Asset => Instance != null ? Instance.asset : null;
    public static SessionManager Session => Instance != null ? Instance.session : null;
    public static DataManager Data => Instance != null ? Instance.data : null;
    public static GameManager Game => Instance != null ? Instance.game : null;
    public static UIManager UI => Instance != null ? Instance.ui : null;
    public static StageManager Stage => Instance != null ? Instance.stage : null;
    public static InventoryManager Inventory => Instance != null ? Instance.inventory : null;
    public static QuestManager Quest => Instance != null ? Instance.quest : null;
    public static SummonManager Summon => Instance != null ? Instance.summon : null;
    public static NotificateManager Notificate => Instance != null ? Instance.notificate : null;
    public static ObjectPoolManager ObjectPool => Instance != null ? Instance.objectPool : null;
    public static SkillDataManager SkillData => Instance != null ? Instance.skillData : null;
    public static FollowerDataManager FollowerData => Instance != null ? Instance.followerData : null;
    public static RankingManager Ranking => Instance != null ? Instance.ranking : null;

    #endregion
}
