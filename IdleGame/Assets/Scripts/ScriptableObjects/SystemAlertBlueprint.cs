using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (fileName = "SystemAlertContainer", menuName ="BluePrints/SystemAlertContainer")]
public class SystemAlertBlueprint : ScriptableObject
{
    public List<MsgAlert> MsgAlert = new List<MsgAlert>();
    public List<PopupAlert> PopupAlert = new List<PopupAlert>();
}

[System.Serializable]
public class MsgAlert
{
    public MsgAlertType AlertType;
    public string ContentMsg;
}

[System.Serializable]
public class PopupAlert
{
    public PopupAlertType AlertType;
    public string Title;
    public string Description;
    public string ConfirmText;
}
