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
        _icon.sprite = Manager.Assets.GetSprite(index);
    }

    public void SlotClear()
    {
        _icon.sprite = null;
    }

    #endregion
}
