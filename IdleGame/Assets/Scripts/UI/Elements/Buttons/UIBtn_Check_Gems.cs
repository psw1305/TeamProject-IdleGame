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

    private int _btnCheckRemain;
    private DateTime _btnCoolTime;

    #endregion

    #region Initialize

    private void Awake()
    {
        _backGround = transform.GetComponent<Image>();
        _payType = transform.Find("Txt_Payment").Find("Image_PayType").GetComponent<Image>();

        _textAction = transform.Find("Txt_Action").GetComponent<TextMeshProUGUI>();
        _textPayment = transform.Find("Txt_Payment").GetComponent<TextMeshProUGUI>();
    }

    public void SetButtonUI(ButtonInfo buttonInfo, Button button)
    {
        _buttonInfo = buttonInfo;
        _button = button;

        _textAction.text = _buttonInfo.BtnText;
        _textPayment.text = _buttonInfo.Amount.ToString();

        if (_buttonInfo.IsLimit)
            _btnCheckRemain = _buttonInfo.LimitCount;
    }

    #endregion

    #region Button Events

    public void ApplyRestriction()
    {
        if (_buttonInfo.IsLimit)
            _btnCheckRemain--;
        if (_buttonInfo.IsCoolDown)
        {
            DateTime.ParseExact(_buttonInfo.CoolTime, "HH:mm:ss", null);
            //StartCoroutine(BtnTimer());
        }

        if (_btnCheckRemain <= 0)
            _button.interactable = false;

    }

    #endregion

    #region Control Methods

    

    #endregion
}
