public class NotificateRecommendWeaponItemSlot : BaseNotiDot
{
    private UserItemData _currentItemData;

    protected override void Start()
    {
        base.Start();
        GetItemData();
        Manager.NotificateDot.SetRecommendWeaponItemNoti += SetRecommendItemNoti;
        SetRecommendItemNoti();
    }

    private void GetItemData()
    {
        _currentItemData = GetComponent<UIPopupEquipSlots>().ItemData;
    }

    private void SetRecommendItemNoti()
    {
        var _recommendItem = Manager.NotificateDot.CheckRecommendItem(Manager.Inventory.WeaponItemList);

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
        if (Manager.NotificateDot != null)
        {
            Manager.NotificateDot.SetRecommendWeaponItemNoti -= SetRecommendItemNoti;
        }
    }
}
