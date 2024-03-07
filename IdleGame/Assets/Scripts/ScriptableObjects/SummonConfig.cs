using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonConfig", menuName = "Blueprints/SummonConfig")]
public class SummonConfig : ScriptableObject
{
    #region Serialize Fields

    [SerializeField] private List<SummonList> summonLists = new List<SummonList>();

    #endregion

    #region Properties

    public List<SummonList> SummonLists => summonLists;

    #endregion
}

