using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.UI
{
    public class LayerControls : MonoBehaviour
    {
        public Button Prev;
        public Button Next;
        public Dropdown Dropdown;
        public Button Hide;
        public Button Paint;
        public Slider Hue;
        public Slider Saturation;
        public Slider Brightness;
        public Transform FixedColors;
        public Action<Color> OnSelectFixedColor;

        public void Start()
        {
            foreach (var button in FixedColors.GetComponentsInChildren<Button>())
            {
                button.onClick.AddListener(() => SelectFixedColor(button.targetGraphic.color));
            }

            var onPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            var onPointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };

            onPointerUp.callback.AddListener(eventData => StartCoroutine(OnDropdownExpanded()));
            onPointerExit.callback.AddListener(eventData => { if (Dropdown.GetComponentInChildren<ScrollRect>() != null) Dropdown.onValueChanged.Invoke(Dropdown.value); });
            Dropdown.GetComponent<EventTrigger>().triggers = new List<EventTrigger.Entry> { onPointerUp, onPointerExit };
        }

        public void SelectFixedColor(Color color)
        {
            OnSelectFixedColor?.Invoke(color);
        }

        private IEnumerator OnDropdownExpanded()
        {
            yield return null;

            var toggles = Dropdown.GetComponentsInChildren<Toggle>().ToList();

            for (var i = 0; i < toggles.Count; i++)
            {
                var index = i;
                var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                var pointerClick = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                var scroll = new EventTrigger.Entry { eventID = EventTriggerType.Scroll };
                var scrollRect = Dropdown.GetComponentInChildren<ScrollRect>();

                pointerEnter.callback.AddListener(eventData => Dropdown.onValueChanged.Invoke(index));
                pointerClick.callback.AddListener(eventData => toggles.ForEach(j => Destroy(j.GetComponent<EventTrigger>())));
                scroll.callback.AddListener(eventData => scrollRect.OnScroll(eventData as PointerEventData));
                toggles[i].gameObject.AddComponent<EventTrigger>().triggers = new List<EventTrigger.Entry> { pointerEnter, pointerClick, scroll };
            }

            if (Dropdown.options.Count > 1)
            {
                Dropdown.GetComponentInChildren<ScrollRect>().verticalScrollbar.value = 1 - (float) Dropdown.value / (Dropdown.options.Count - 1);
            }
        }
    }
}