using DG.Tweening;
using UnityEngine;

public static class ExtensionsRectTransform
{
    public static void YoyoScale(this RectTransform rectTransform, Vector2 to, float duration)
    {
        rectTransform
            .DOScale(to, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public static void BounceScale(this RectTransform rectTransform, float duration)
    {
        rectTransform
            .DOScale(1f, duration)
            .SetEase(Ease.OutBounce);
    }

    public static void LerpAlpha(this CanvasGroup canvasGroup, float to, float duration)
    {
        canvasGroup.
            DOFade(to, duration);
    }
}
