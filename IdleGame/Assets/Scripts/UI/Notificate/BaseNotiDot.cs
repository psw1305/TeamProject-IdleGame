using UnityEngine;

public class BaseNotiDot : MonoBehaviour
{
    [SerializeField] private int order = 25;
    public GameObject ChildNotiDot;

    protected virtual void Start()
    {
        InitNotificateUI();
    }

    public void InitNotificateUI()
    {
        ChildNotiDot = Manager.Asset.InstantiatePrefab("NotificationDot", gameObject.transform);
        ChildNotiDot.transform.localPosition = new Vector2(GetComponent<RectTransform>().rect.width / 2 - 10, GetComponent<RectTransform>().rect.height / 2 - 10);
        ChildNotiDot.GetComponent<Canvas>().sortingOrder = order;
    }

    protected void ActiveNotiDot()
    {
        if (ChildNotiDot != null)
        {
            ChildNotiDot.SetActive(true);
        }
    }

    protected void InactiveNotiDot()
    {
        if (ChildNotiDot != null)
        {
            ChildNotiDot.SetActive(false);
        }
    }
}
