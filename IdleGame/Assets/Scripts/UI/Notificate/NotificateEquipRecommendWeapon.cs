public class NotificateEquipRecommendWeapon : BaseNotiDot
{
    private UserItemData _currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.Notificate.SetRecommendWeaponItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        _currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        var _recommendItem = Manager.Notificate.CheckRecommendItem(Manager.Inventory.WeaponItemList);

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
            Manager.Notificate.SetRecommendWeaponItemNoti -= SetRecommendItemNoti;
        }
    }
}
