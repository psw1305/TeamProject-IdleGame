// 유저 인게임 스텟 정보
public class StatInfo
{
    public string Id;
    public int Level;
    public long BaseValue;
    public long BaseUpgradeCost;

    public long Value;
    public long UpgradeCost;
    public long Modifier;
    
    public StatModType ModType;

    public StatInfo(string id, int level, long baseValue, long modifier, StatModType modType)
    {
        Id = id;
        Level = level;
        BaseValue = baseValue;
        BaseUpgradeCost = 50;
        Modifier = modifier;
        ModType = modType;

        StatLevelCalculate();
    }

    public void AddModifier()
    {
        Level += 1;
        StatLevelCalculate();
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
            return Utilities.ConvertToString(Value); 
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
