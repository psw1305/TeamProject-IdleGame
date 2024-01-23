using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.UI
{
    public class Popup : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public Text Message;

        public static Popup Instance;

        public void Awake()
        {
            Instance = this;
        }

        public void Show(string message)
        {
            CanvasGroup.alpha = 1;
            Message.text = message;
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(2);

            while (CanvasGroup.alpha > 0)
            {
                CanvasGroup.alpha -= Time.deltaTime;

                yield return null;
            }
        }
    }
}