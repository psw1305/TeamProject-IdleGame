using System.Collections;
using System.Collections.Generic;
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
        // 초기 플레이 시 => 작동 정상
        // Error => 상점에서 아이템 뽑은 후 => 장비 버튼 누르면 itemContainer 안에 오브젝트들이 오류

        ChildNotiDot.SetActive(false);
    }
}
