using UnityEngine;
using DG.Tweening;

public class UINotiDot : MonoBehaviour
{
    [SerializeField] Vector2 scaleTo;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.YoyoScale(scaleTo, 0.5f);
    }

    private void OnDestroy()
    {
        rectTransform.DOKill();
    }
}
