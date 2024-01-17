using UnityEngine;

// 유저 인게임 스텟 정보
public class StatInfo
{
    public int Level;
    public long Value;
    public long UpgradeCost;
    public long Modifier;
    public StatModType ModType;

    public StatInfo(int level, long value, long upgradeCost, long modifier, StatModType modType)
    {
        Level = level;
        Value = value;
        UpgradeCost = upgradeCost;
        Modifier = modifier;
        ModType = modType;
    }

    public void AddModifier()
    {
        Value += Modifier;
    }

    public float GetFloat()
    {
        return Value / 1000f;
    }

    public string GetString()
    {
        if (ModType == StatModType.Percent)
        {
            float percent = Value / 10f;
            return $"{percent}%";
        }
        else if (ModType == StatModType.DecimalPoint)
        {
            float decimalPoint = Value / 1000f;
            return decimalPoint.ToString();
        }
        else
        { 
            return Value.ToString(); 
        }
    }
}
