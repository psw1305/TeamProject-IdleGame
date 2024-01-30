using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonList", menuName = "Blueprints/SummonList")]
public class SummonList : ScriptableObject
{
    #region Fields

    [Header("Info")]
    [SerializeField] private string summonName;
    [SerializeField] private GameObject banner;
    [SerializeField] private string tableLink;

    [Header("Button Info")]
    [Header("Button 1")]
    [Tooltip("속성을 연결할 버튼 프리팹 이름")]
    [SerializeField] private string btnPrefab_1;
    [SerializeField] private ResourceType paymentType_1;
    [SerializeField] private int amount_1;
    [SerializeField] private int summonCount_1;

    [Header("Button 2")]
    [SerializeField] private string btnPrefab_2;
    [SerializeField] private ResourceType paymentType_2;
    [SerializeField] private int amount_2;
    [SerializeField] private int summonCount_2;

    [Header("Button 3")]
    [SerializeField] private string btnPrefab_3;
    [SerializeField] private ResourceType paymentType_3;
    [SerializeField] private int amount_3;
    [SerializeField] private int summonCount_3;

    #endregion

    #region Properties

    public string SummonName => summonName;
    public GameObject Banner => banner;
    public string TableLink => tableLink;

    public string BtnPrefab_1 => btnPrefab_1;
    public ResourceType PaymentType_1 => paymentType_1;
    public int Amount_1 => amount_1;
    public int SummonCount_1 => summonCount_1;

    public string BtnPrefab_2 => btnPrefab_2;
    public ResourceType PaymentType_2 => paymentType_2;
    public int Amount_2 => amount_2;
    public int SummonCount_2 => summonCount_2;

    public string BtnPrefab_3 => btnPrefab_3;
    public ResourceType PaymentType_3 => paymentType_3;
    public int Amount_3 => amount_3;
    public int SummonCount_3 => summonCount_3;


    #endregion
}
