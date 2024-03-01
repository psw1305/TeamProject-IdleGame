using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeIO : UIBase
{
    #region Fields

    private Image _image;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetUI<Image>();
        _image = GetUI<Image>("Image");
    }

    #endregion

    #region Effect Method

    public void FadeEffect(float duration, float inItAlpha = 0.0f, float setAlpha = 1.0f)
    {
        _image.color = new Color(0f, 0f, 0f, inItAlpha);
        _image
            .DOFade(setAlpha, duration)
            .OnComplete(() =>
            {
                Manager.Stage.EffectControl = true;
            }            
            )
            .SetEase(Ease.InOutSine);
    }

    public void FadeReset()
    {
        _image.color = new Color(0f, 0f, 0f, 0f);
    }

    #endregion
}
