using UnityEngine;
using UnityEngine.UI;

public class UIRewardsItemSlot : MonoBehaviour
{
    #region Fields

    private Image icon;
    private ItemContainerBlueprint itemContainer;
    private SkillContainerBlueprint skillContainer;
    private FollowerContainerBlueprint followerContainer;

    #endregion

    #region Initialize

    private void Awake()
    {
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
        }
        else if (itemType == "Skills")
        {
            icon.sprite = skillContainer.FindSprite(id);
        }
        else if (itemType == "Follower")
        {
            icon.sprite = followerContainer.FindSprite(id);
        }
    }

    public void SlotClear()
    {
        icon.sprite = null;
    }

    #endregion
}
