using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBtn_Check_Gems : MonoBehaviour
{
    #region Serialize Field

    [SerializeField] private TextMeshProUGUI textAction;
    [SerializeField] private TextMeshProUGUI textPayment;
    [SerializeField] private Image payType;

    #endregion

    #region Fields

    private ButtonInfo buttonInfo;
    private Button button;

    private DateTime _btnCoolTime;

    private int summonCountAdd;

    #endregion

    #region Properties

    public int BtnCheckRemain { get; private set; }
    public bool Interactive => button.interactable;
    public ButtonInfo ButtonInfo => buttonInfo;

    #endregion

    #region Initialize

    public void SetButtonUI(ButtonInfo buttonInfo, Button button, int summonCountAdd)
    {
        this.buttonInfo = buttonInfo;
        this.button = button;
        this.summonCountAdd = summonCountAdd;

        BtnCheckRemain = -1;
        this.button.interactable = true;

        if (this.buttonInfo.IsLimit)
        {
            BtnCheckRemain = this.buttonInfo.LimitCount;
        }

        textAction.text = string.Format(this.buttonInfo.BtnText, this.buttonInfo.SummonCount + this.summonCountAdd);
        UpdatePaymentText();
    }

    #endregion

    #region Button Events

    public void ApplyRestriction()
    {
        if (buttonInfo.IsLimit)
        {
            BtnCheckRemain--;
        }

        if (buttonInfo.IsCoolDown)
        {
            DateTime.ParseExact(buttonInfo.CoolTime, "HH:mm:ss", null);
        }

        if (BtnCheckRemain <= 0 && BtnCheckRemain != -1)
        {
            button.interactable = false;
        }
    }

    #endregion

    #region Control Methods

    public void ApplySummonCountAdd(int summonCountAdd)
    {
        this.summonCountAdd = summonCountAdd;
    }

    public void UpdateUI()
    {
        textAction.text = String.Format(buttonInfo.BtnText, buttonInfo.SummonCount + summonCountAdd);
        UpdatePaymentText();
    }

    private void UpdatePaymentText()
    {
        if (buttonInfo.Amount < 1 && buttonInfo.IsLimit)
        {
            textPayment.text = $"{BtnCheckRemain}/{buttonInfo.LimitCount}";
        }
        else
        {
            textPayment.text = buttonInfo.Amount.ToString();
        }
    }

    #endregion
}
