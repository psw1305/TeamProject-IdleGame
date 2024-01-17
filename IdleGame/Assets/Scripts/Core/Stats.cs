// 유저 인게임 스텟 정보
public class StatInfo
{
    public int Level;
    public long UpgradeCost;
    public float Modifier; // 추가적인 스탯 증가를 위한 Modifier

    public StatInfo(int level, long cost)
    {
        this.Level = level;
        this.UpgradeCost = cost;
    }
}
