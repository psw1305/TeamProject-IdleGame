public class NotificateEquipRecommendArmor : BaseNotiDot
{
    private UserItemData _currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.Notificate.SetRecommendArmorItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        _currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        var _recommendItem = Manager.Notificate.CheckRecommendItem(Manager.Inventory.ArmorItemList);
        if (_recommendItem == null || _recommendItem.equipped)
        {
            InactiveNotiDot();
            return;
        }
        if (_currentItemData == _recommendItem)
        {
            ActiveNotiDot();
        }
        else if (_currentItemData != _recommendItem)
        {
            InactiveNotiDot();
        }
    }

    private void OnDestroy()
    {
        if (Manager.Notificate != null)
        {
            Manager.Notificate.SetRecommendArmorItemNoti -= SetRecommendItemNoti;
        }
    }
}
