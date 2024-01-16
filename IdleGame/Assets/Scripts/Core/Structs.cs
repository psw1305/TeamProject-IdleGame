// 플레이어 업그레이드
public struct UpgradeInfo
{
    public int Level;
    public long UpgradeCost;

    public UpgradeInfo(int Lv, long Cost)
    {
        this.Level = Lv;
        this.UpgradeCost = Cost;
    }

    public void SetModifier(int Lv, long Cost)
    {
        this.Level += Lv;
        this.UpgradeCost += Cost;
    }
}