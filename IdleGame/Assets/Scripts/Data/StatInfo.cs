using UnityEngine;

// 유저 인게임 스텟 정보
public class StatInfo
{
    public int Level;
    public long BaseValue;
    public long BaseUpgradeCost;

    public long Value;
    public long UpgradeCost;
    public long Modifier;
    
    public StatModType ModType;

    public StatInfo(int level, long baseValue, long modifier, StatModType modType)
    {
        Level = level;
        BaseValue = baseValue;
        BaseUpgradeCost = 50;
        Modifier = modifier;
        ModType = modType;

        StatLevelCalculate();
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

    #region Calculate

    public void StatLevelCalculate()
    {
        Value = BaseValue + Modifier * (Level - 1);
        UpgradeCost = BaseUpgradeCost + 10 * (Level - 1);
    }

    #endregion
}
