using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
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
    public void OnPointerEnter(PointerEventData eventData) => InvokeEventAction(UIEventType.PointerEnter, eventData);
    public void OnPointerExit(PointerEventData eventData) => InvokeEventAction(UIEventType.PointerExit, eventData);
    public void OnDrag(PointerEventData eventData) => InvokeEventAction(UIEventType.Drag, eventData);

    private void OnDestroy()
    {
        _eventHandlers.Clear();
    }
}
