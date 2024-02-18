using UnityEngine;

public class BaseNotiDot : MonoBehaviour
{
    private GameObject ChildNotiDot;

    protected virtual void Start()
    {
        InitNotificateUI();
    }

    public void InitNotificateUI()
    {
        ChildNotiDot = Manager.Address.InstantiatePrefab("NotificationDot", gameObject.transform);
        ChildNotiDot.transform.localPosition = new Vector2(GetComponent<RectTransform>().rect.width / 2 - 10, GetComponent<RectTransform>().rect.height / 2 - 10);
    }

    protected void ActiveNotiDot()
    {
        ChildNotiDot.SetActive(true);
    }

    protected void InactiveNotiDot()
    {
        if (ChildNotiDot != null)
        {
            ChildNotiDot.SetActive(false);
        }
    }
}
