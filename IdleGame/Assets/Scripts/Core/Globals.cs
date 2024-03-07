using System;

public static class BaseStat
{
    public const int AtkDamage = 10;
    public const int AtkSpeed = 500;
    public const int CritChance = 0;
    public const int CritDamage = 100;
    public const int Hp = 1000;
    public const int HpRecovery = 80;
}

public static class BaseUpgradeCost
{
    public const int AtkDamage = 12;
    public const int AtkSpeed = 10;
    public const int CritChance = 1000;
    public const int CritDamage = 10;
    public const int Hp = 6;
    public const int HpRecovery = 4;
}

public static class StatModifier
{
    public const int AtkDamage = 10;
    public const int AtkSpeed = 10;
    public const int CritChance = 1;
    public const int CritDamage = 1;
    public const int Hp = 100;
    public const int HpRecovery = 8;
}

public static class CostModifier
{
    public const int AtkDamage = 6;
    public const int AtkSpeed = 60;
    public const int CritChance = 20;
    public const int CritDamage = 6;
    public const int Hp = 6;
    public const int HpRecovery = 6;
}

public static class Delay
{
    public static readonly TimeSpan IdleClick = new(0, 5, 0);
    public static readonly TimeSpan BonusClick = new(0, 30, 0);
}
