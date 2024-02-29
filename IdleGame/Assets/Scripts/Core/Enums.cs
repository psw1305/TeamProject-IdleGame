// 다운로드 사이즈 단위
public enum SizeUnits
{
    Bytes, KB, MB, GB
}

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
    IntegerPercent,
}

// 스탯 적용 타입
public enum StatApplyType
{
    linear,
    EnhancedLinear
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

public enum PaymentType
{
    Resource,
    advert
}

public enum ResourceType
{
    Gold,
    Gems
}

public enum EnemyType
{
    Normal,
    Boss
}

public enum PlayerState
{
    None,
    Move,
    Battle,
    Die
}

public enum StatType
{
    Attack,
    HP
}

public enum ItemTier
{
    Common,
    Uncommon,
    Rare,
    epic,
    Legendary
}

public enum MsgAlertType
{
    NeedMoreGold,
    NeedMoreGem,
    CanNotEquip,
    CanNotReinforce,
    ItemLimitLevel,
    CanNotAllReinforce
}

public enum PopupAlertType
{
    ApplicationQuit,
    DisconnectNetwork,
    DevelopingContent
}