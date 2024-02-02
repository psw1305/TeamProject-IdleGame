using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private readonly Dictionary<UIEventType, Action<PointerEventData>> _eventHandlers = new();

    private void InvokeEventAction(UIEventType uiEventType, PointerEventData eventData)
    {
        if (_eventHandlers.TryGetValue(uiEventType, out var action))
        {
            action?.Invoke(eventData);
        }
    }

    public void BindEvent(UIEventType uiEventType, Action<PointerEventData> action)
    {
        _eventHandlers[uiEventType] = action;
    }

    public void UnbindEvent(UIEventType uiEventType)
    {
        if (_eventHandlers.ContainsKey(uiEventType))
        {
            _eventHandlers.Remove(uiEventType);
        }
    }

    public void OnPointerClick(PointerEventData eventData) => InvokeEventAction(UIEventType.Click, eventData);
    public void OnPointerDown(PointerEventData eventData) => InvokeEventAction(UIEventType.PointerDown, eventData);
    public void OnPointerUp(PointerEventData eventData) => InvokeEventAction(UIEventType.PointerUp, eventData);
    public void OnDrag(PointerEventData eventData) => InvokeEventAction(UIEventType.Drag, eventData);

    private void OnDestroy()
    {
        _eventHandlers.Clear();
    }
}
