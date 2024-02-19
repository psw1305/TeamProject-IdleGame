using UnityEngine;
using UnityEngine.UI;

public class UIRewardsItemSlot : MonoBehaviour
{
    #region Fields

    private Image _icon;

    #endregion

    #region Initialize

    private void Awake()
    {
        _icon = transform.Find("ItemIcon").GetComponent<Image>();
    }

    public void UpdateSlot(string index)
    {
        // TODO => 아이템 데이터에서 이미지 가져오도록
        //_icon.sprite = Manager.Asset.GetSprite(index);
    }

    public void SlotClear()
    {
        _icon.sprite = null;
    }

    #endregion
}
