using System.Collections;
using TMPro;
using UnityEngine;

public class SystemAlertFloating : MonoBehaviour
{
    private static SystemAlertFloating instance;


    public static SystemAlertFloating Instance
    {
        get
        {
            if (instance == null)
            {
                var go = Instantiate(Manager.Asset.GetPrefab("UIMsgSystemAlert"));
                instance = go.GetComponent<SystemAlertFloating>();
            }
            return instance;
        }
    }

    private Coroutine _routine;
    [SerializeField] private CanvasGroup boarder;
    [SerializeField] private TextMeshProUGUI cantentText;

    public void ShowMsgAlert(MsgAlertType alertType)
    {
        cantentText.text = Manager.SysAlert.MsgAlerts[alertType].ContentMsg;

        if (_routine != null)
        {
            StopCoroutine(_routine);
        }
        _routine = StartCoroutine(ShowMsg());
    }

    private IEnumerator ShowMsg()
    {
        boarder.alpha = 1;
        yield return new WaitForSecondsRealtime(1f);
        ExtensionsRectTransform.LerpAlpha(boarder, 0, 0.25f);
    }
}
