using UnityEngine;

public class NotiDotParentBtn : MonoBehaviour
{
    public GameObject ChildNotiDot;
    public void ActiveNotiDot()
    {
        ChildNotiDot.SetActive(true);
    }

    public void InactiveNotiDot()
    {
        ChildNotiDot.SetActive(false);
    }
}
