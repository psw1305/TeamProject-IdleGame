using UnityEngine;

public class BaseNotiDot : MonoBehaviour
{
    public GameObject ChildNotiDot;

    protected virtual void Start()
    {
        InitNotificateUI();
    }

    public void InitNotificateUI()
    {
        GameObject _childDot = Manager.Resource.InstantiatePrefab("NotiDot", gameObject.transform);
        ChildNotiDot = _childDot;
        _childDot.gameObject.transform.localPosition = new Vector2(GetComponent<RectTransform>().rect.width / 2 - 10, GetComponent<RectTransform>().rect.height / 2 - 10);
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
