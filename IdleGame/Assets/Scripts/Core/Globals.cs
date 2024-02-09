using System;

public static class AssetPath
{
}

public static class BaseStat
{
    public const int AtkDamage = 10;
    public const int AtkSpeed = 500;
    public const int CritChance = 0;
    public const int CritDamage = 1000;
    public const int Hp = 1000;
    public const int HpRecovery = 30;
}

public static class Delay
{
    public static readonly TimeSpan IdleClick = new(0, 1, 0);
    public static readonly TimeSpan BonusClick = new(0, 10, 0);
}
