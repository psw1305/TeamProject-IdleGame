using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIEvent
{
    public static void SetEvent(this GameObject gameObject, UIEventType uiEventType, Action<PointerEventData> action)
    {
        UIEventHandler handler = Utilities.GetOrAddComponent<UIEventHandler>(gameObject);
        handler.BindEvent(uiEventType, action);
    }
}
