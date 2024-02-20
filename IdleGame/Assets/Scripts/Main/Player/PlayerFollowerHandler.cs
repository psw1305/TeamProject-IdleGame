using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowerHandler : MonoBehaviour
{
    [SerializeField] private Transform[] _followerPosition;

    private Dictionary<int, EquipFollowerData> _userEquipFollowerSlot = new Dictionary<int, EquipFollowerData>();

    private event Action FollowerWalk;
    private event Action FollowerBattle;

    public void AddFollowerWalkEvent(Action action)
    {
        FollowerWalk += action;
    }
    public void RemoveFollowerWalkEvent(Action action) 
    {
        FollowerWalk -= action;
    }
    public void InvokeFollowerWalk()
    {
        FollowerWalk?.Invoke ();
    }

    public void AddFollowerBattleEvent(Action action)
    {
        FollowerBattle += action;
    }
    public void RemoveFollowerBattleEvent(Action action)
    {
        FollowerBattle -= action;
    }
    public void InvokeFollowerBattle()
    {
        FollowerBattle?.Invoke ();
    }

    public void InitFollowerSlot()
    {
        int equipslotIndex = 0;
        foreach (var item in Manager.Data.FollowerData.UserEquipFollower)
        {
            var go = new GameObject("FollowerObj");
            go.transform.parent = transform;
            _userEquipFollowerSlot.Add(equipslotIndex, go.AddComponent<EquipFollowerData>());
            _userEquipFollowerSlot[equipslotIndex].SetFollowerObject(Manager.Data.FollowerData.UserEquipFollower[equipslotIndex].itemID, _followerPosition[equipslotIndex], equipslotIndex);
            equipslotIndex++;
        }
    }

    public void ChangeEquipFollowerData(int slotIndex)
    {
        _userEquipFollowerSlot[slotIndex].SetFollowerObject(Manager.Data.FollowerData.UserEquipFollower[slotIndex].itemID, _followerPosition[slotIndex], slotIndex);        
    }
}

public class EquipFollowerData: MonoBehaviour
{
    public GameObject FollowerObject { get; private set; }

    public Follower FollowerScript { get; private set; }

    public void SetFollowerObject(string itemID, Transform spawntransform, int slotIndex)
    {
        if(itemID == "Empty")
        {
            if(FollowerObject != null)
            {
                Destroy(FollowerObject);
                FollowerScript = null;
            }
            return;
        }

        if(FollowerObject != null)
        {
            Destroy (FollowerObject);
            FollowerScript = null;
        }

        var followerBlueprint = Manager.FollowerData.FollowerDataDictionary[itemID];
        FollowerObject = Manager.Asset.InstantiatePrefab("FollowerFrame", spawntransform);
        FollowerObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = slotIndex;
        FollowerScript = FollowerObject.GetComponent<Follower>();
        FollowerScript.Initialize(followerBlueprint);
    }
}