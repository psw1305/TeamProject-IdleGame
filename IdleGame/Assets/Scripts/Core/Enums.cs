// UI Event 타입 열거형
public enum UIEventType
{
    Click, PointerDown, PointerUp, Drag,
}

// 스탯 계산 타입
public enum StatModType
{
    Integer,
    Percent,
    DecimalPoint,
}

// 퀘스트 타입 열거형
public enum QuestType
{
    DamageUp,
    HPUp,
    DefeatEnemy,
    StageClear,
    None,
    //ItemGamble    
}

public enum EquipFillterType
{
    Weapon,
    Armor
}

public enum DamageType
{
    Normal,
    Critical
}