using UnityEngine;

[CreateAssetMenu(fileName = "FollowerBlueprint", menuName = "Blueprints/Follower")]
public class FollowerBlueprint : ScriptableObject
{
    [SerializeField] private string itemID;
    [SerializeField] private GameObject followerObject;

    public string ItemID => itemID;
    public GameObject FollowerObject => followerObject;
}
