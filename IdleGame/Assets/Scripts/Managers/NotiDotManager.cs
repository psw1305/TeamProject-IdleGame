using System.Collections.Generic;
using UnityEngine;

public class NotiDotManager
{
    private List<NotiDotParentBtn> AllReinforceNotificateList = new List<NotiDotParentBtn>();

    public void AddAllReinforceNotificateObject(Transform parent)
    {
        //이미 알림용 닷이 있으면 생성하지 않음 
        if (parent.GetComponent<NotiDotParentBtn>() == null)
        {
            NotiDotParentBtn noti = parent.gameObject.AddComponent<NotiDotParentBtn>();
            GameObject _childDot = Manager.Resource.InstantiatePrefab("NotiDot", parent);
            noti.ChildNotiDot = _childDot;
            _childDot.gameObject.transform.localPosition = new Vector2(noti.GetComponent<RectTransform>().rect.width / 2, noti.GetComponent<RectTransform>().rect.height / 2);
        }
        AllReinforceNotificateList.Add(parent.GetComponent<NotiDotParentBtn>());
        Debug.LogWarning(parent.gameObject.name);
    }

    public void RemoveAllReinforceNotificateObject(Transform parent)
    {
        AllReinforceNotificateList.Remove(parent.GetComponent<NotiDotParentBtn>());
    }

    public void ActiveNotiMarker()
    {
        foreach (var obj in AllReinforceNotificateList)
        {
            obj.ActiveNotiDot();
        }
    }

    public void InactiveNotiMarker()
    {
        foreach (var obj in AllReinforceNotificateList)
        {
            obj.InactiveNotiDot();
        }
    }
    
    //조건문도 추가해야함.
}