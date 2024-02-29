using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAlertDataManager
{
    private Dictionary<MsgAlertType, MsgAlert> _msgAlerts = new Dictionary<MsgAlertType, MsgAlert>();
    private Dictionary<PopupAlertType, PopupAlert> _popupAlerts = new Dictionary<PopupAlertType, PopupAlert>();
    public Dictionary<MsgAlertType, MsgAlert> MsgAlerts => _msgAlerts;
    public Dictionary<PopupAlertType, PopupAlert> PopupAlerts => _popupAlerts;

    public void InitAlert()
    {
        var blueprint = Manager.Asset.GetBlueprint("SystemAlertContainer") as SystemAlertBlueprint;

        foreach (var content in blueprint.MsgAlert)
        {
            _msgAlerts.Add(content.AlertType, content);
        }
        foreach (var content in blueprint.PopupAlert)
        {
            _popupAlerts.Add(content.AlertType, content);
        }
    }
}
