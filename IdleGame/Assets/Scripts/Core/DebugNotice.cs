using UnityEngine;
using TMPro;

public class DebugNotice : MonoBehaviour
{
    #region Singleton

    public static DebugNotice Instance { get; private set; }

    #endregion

    [SerializeField] private TextMeshProUGUI noticeText;

    private void Awake()
    {
        Instance = this;
    }

    public void Notice(string notice)
    {
        noticeText.text = notice;
    }
}