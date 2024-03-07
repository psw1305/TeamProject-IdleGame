using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIRewardsItemSlot : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image bgImg;
    [SerializeField] private Image icon;
    private ItemTier _tier;
    private ItemContainerBlueprint itemContainer;
    private SkillContainerBlueprint skillContainer;
    private FollowerContainerBlueprint followerContainer;
    private RectTransform rectTransform;

    #endregion

    #region Initialize

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;

        itemContainer = Manager.Asset.GetBlueprint("ItemDataContainer") as ItemContainerBlueprint;
        skillContainer = Manager.Asset.GetBlueprint("SkillDataContainer") as SkillContainerBlueprint;
        followerContainer = Manager.Asset.GetBlueprint("FollowerDataContainer") as FollowerContainerBlueprint;
    }

    public void UpdateSlot(string itemType, string id)
    {
        rectTransform.BounceScale(0.25f);

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

        bgImg.color = Utilities.SetSlotTierColor(_tier);
    }

    private void OnDestroy()
    {
        rectTransform.DOKill();
    }

    #endregion
}
