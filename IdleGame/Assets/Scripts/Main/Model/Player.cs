using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties

    public int Damage { get; private set; }
    public int Hp { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CriticalPercent { get; private set; }
    public float CriticalDamage { get; private set; }
    public float Range { get; private set; }

    #endregion

    #region Init

    private void Start()
    {
        Damage = 10;
        Hp = 10;
        AttackSpeed = 0.10f;
        CriticalPercent =  0.00f;
        CriticalDamage = 0;
        Range = 10;
    }

    #endregion
}
