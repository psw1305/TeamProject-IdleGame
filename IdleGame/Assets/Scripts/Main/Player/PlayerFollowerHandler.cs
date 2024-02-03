using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollowerHandler : MonoBehaviour
{
    private Dictionary<int, EquipFollowerData> _userEquipFollowerSlot = new Dictionary<int, EquipFollowerData>();

    private void Start()
    {
        InitFollowerSlot();
    }

    private void InitFollowerSlot()
    {
        int equipslotIndex = 0;
        foreach (var item in Manager.Data.FollowerData.UserEquipFollower)
        {
            var go = new GameObject("FollowerObj");
            go.transform.parent = transform;
            _userEquipFollowerSlot.Add(equipslotIndex, go.AddComponent<EquipFollowerData>());
            _userEquipFollowerSlot[equipslotIndex].SetFollowerObject(Manager.Data.UserSkillData.UserEquipSkill[equipslotIndex].itemID);
            equipslotIndex++;
        }
    }
}

public class EquipFollowerData: MonoBehaviour
{
    public GameObject FollowerObject { get; private set; }

    public Follower FollowerScript { get; private set; }

    public void SetFollowerObject(string itemID)
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

        FollowerScript = FollowerObject.GetComponent<Follower>();

    }
}

