using UnityEngine;

[CreateAssetMenu(fileName = "Follower", menuName = "Blueprints/Follower")]
public class FollowerBlueprint : ScriptableObject
{

    [SerializeField] private string itemID;
    [SerializeField] private GameObject followerObject;

    public string ItemID => itemID;
    public GameObject FollowerObject => followerObject;
}
