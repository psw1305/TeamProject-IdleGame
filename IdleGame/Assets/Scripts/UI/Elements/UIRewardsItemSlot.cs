using UnityEngine;
using UnityEngine.UI;

public class UIRewardsItemSlot : MonoBehaviour
{
    #region Fields

    private Image _bgImg;
    private ItemTier _tier;
    private Image icon;
    private ItemContainerBlueprint itemContainer;
    private SkillContainerBlueprint skillContainer;
    private FollowerContainerBlueprint followerContainer;

    #endregion

    #region Initialize

    private void Awake()
    {
        _bgImg = GetComponent<Image>();
        icon = transform.Find("ItemIcon").GetComponent<Image>();
        itemContainer = Manager.Asset.GetBlueprint("ItemDataContainer") as ItemContainerBlueprint;
        skillContainer = Manager.Asset.GetBlueprint("SkillDataContainer") as SkillContainerBlueprint;
        followerContainer = Manager.Asset.GetBlueprint("FollowerDataContainer") as FollowerContainerBlueprint;
    }

    public void UpdateSlot(string itemType, string id)
    {
        if (itemType == "Equipment")
        {
            icon.sprite = itemContainer.FindSprite(id);
            _tier = itemContainer.itemDatas.Find(item => item.ItemID == id).Rarity;
        }
        else if (itemType == "Skills")
        {
            icon.sprite = skillContainer.FindSprite(id);
            _tier = skillContainer.skillDatas.Find(item => item.ItemID == id).Rarity;
        }
        else if (itemType == "Follower")
        {
            icon.sprite = followerContainer.FindSprite(id);
            _tier = followerContainer.followerDatas.Find(item => item.ItemID == id).Rarity;
        }
        _bgImg.color = Utilities.SetSlotTierColor(_tier);
    }

    public void SlotClear()
    {
        icon.sprite = null;
    }

    #endregion
}
