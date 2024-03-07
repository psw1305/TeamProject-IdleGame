using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonList", menuName = "Blueprints/SummonList")]
public class SummonList : ScriptableObject
{
    #region Properties

    public List<ButtonInfo> ButtonInfo => _buttonInfo;

    #endregion

    #region Fields

    [Header("Info")]
    [SerializeField] private string summonName;
    [SerializeField] private GameObject banner;
    [SerializeField] private string typeLink;
    [SerializeField] private int countAddLimit;

    #endregion

    #region Serialize Fields

    [SerializeField] private List<ButtonInfo> _buttonInfo = new List<ButtonInfo>();

    #endregion

    #region Properties

    public string SummonName => summonName;
    public GameObject Banner => banner;
    public string TypeLink => typeLink;
    public int CountAddLimit => countAddLimit;

    #endregion
}

[Serializable]
public class ButtonInfo
{
    #region Serialize Fields

    [Tooltip("스크립트와 연결할 오브젝트의 이름을 정확히 적어주세요")]
    [SerializeField] private string btnPrefab;
    [Tooltip("인게임에서 표기될 버튼 텍스트")]
    [SerializeField] private string btnText;
    [SerializeField] private PaymentType paymentType;
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int amount;
    [SerializeField] private int summonCount;
    [Tooltip("해당 버튼에서 별도로 실행할 동작이 있는 경우")]
    [SerializeField] private bool onEvent;
    [Space(20)]
    [SerializeField] private bool isLimit;
    [SerializeField] private int limitCount;
    [Space(20)]
    [SerializeField] private bool isCoolDown;
    [Tooltip("string 형식입니다. HH:mm:ss")]
    [SerializeField] private string coolTime;

    #endregion

    #region Properties

    public string BtnPrefab => btnPrefab;
    public string BtnText => btnText;
    public PaymentType PaymentType => paymentType;
    public ResourceType ResourceType => resourceType;
    public int Amount => amount;
    public int SummonCount => summonCount;
    public bool OnEvent => onEvent;
    public bool IsLimit => isLimit;
    public int LimitCount => limitCount;
    public bool IsCoolDown => isCoolDown;
    public string CoolTime => coolTime;

    #endregion
}
