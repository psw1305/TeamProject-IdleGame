using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class UIBtn_Check_Gems : MonoBehaviour
{
    #region Fields

    private ButtonInfo _buttonInfo;
    private Button _button;

    private Image _backGround;
    private Image _payType;

    private TextMeshProUGUI _textAction;
    private TextMeshProUGUI _textPayment;

    private DateTime _btnCoolTime;

    private int _summonCountAdd;

    #endregion

    #region Properties

    public int BtnCheckRemain { get; private set; }
    public bool Interactive => _button.interactable;
    public ButtonInfo ButtonInfo => _buttonInfo;

    #endregion

    #region Initialize

    private void Awake()
    {
        _backGround = transform.GetComponent<Image>();
        _payType = transform.Find("Txt_Payment").Find("Image_PayType").GetComponent<Image>();

        _textAction = transform.Find("Txt_Action").GetComponent<TextMeshProUGUI>();
        _textPayment = transform.Find("Txt_Payment").GetComponent<TextMeshProUGUI>();
    }

    public void SetButtonUI(ButtonInfo buttonInfo, Button button, int summonCountAdd)
    {
        _buttonInfo = buttonInfo;
        _button = button;
        _summonCountAdd = summonCountAdd;

        BtnCheckRemain = -1;
        _button.interactable = true;

        if (_buttonInfo.IsLimit)
            BtnCheckRemain = _buttonInfo.LimitCount;

        _textAction.text = String.Format(_buttonInfo.BtnText, _buttonInfo.SummonCount + _summonCountAdd);
        UpdatePaymentText();
    }

    #endregion

    #region Button Events

    public void ApplyRestriction()
    {
        if (_buttonInfo.IsLimit)
            BtnCheckRemain--;
        if (_buttonInfo.IsCoolDown)
        {
            DateTime.ParseExact(_buttonInfo.CoolTime, "HH:mm:ss", null);
            //StartCoroutine(BtnTimer());
        }

        if (BtnCheckRemain <= 0 && BtnCheckRemain != -1)
            _button.interactable = false;

    }

    #endregion

    #region Control Methods

    public void ApplySummonCountAdd(int summonCountAdd)
    {
        _summonCountAdd = summonCountAdd;
    }

    public void UpdateUI()
    {
        _textAction.text = String.Format(_buttonInfo.BtnText, _buttonInfo.SummonCount + _summonCountAdd);
        UpdatePaymentText();
    }

    private void UpdatePaymentText()
    {
        if (_buttonInfo.Amount < 1 && _buttonInfo.IsLimit)
        {
            _textPayment.text = $"{BtnCheckRemain}/{_buttonInfo.LimitCount}";
        }
        else
        {
            _textPayment.text = _buttonInfo.Amount.ToString();
        }
    }

    #endregion
}
