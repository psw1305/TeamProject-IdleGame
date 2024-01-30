using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonConfig", menuName = "Blueprints/SummonConfig")]
public class SummonBlueprint : ScriptableObject
{
    [SerializeField] private List<SummonList> summonLists = new List<SummonList>();

    public List<SummonList> SummonLists => summonLists;
}

