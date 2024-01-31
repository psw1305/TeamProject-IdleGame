using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : UIBase
{
    #region Serialize Fields

    [SerializeField] private Canvas UICanvas;

    #endregion

    #region Fields

    private Image _hpBackGround;
    private Image _hpBar;
    private Image _hpBarDamage;
    private TextMeshProUGUI _hpBarText;

    private Vector2 _normalSize = new Vector2(0.6f, 0.1f);
    private Vector2 _bossSize = new Vector2(0.8f, 0.2f);

    private Coroutine _damageEffectCoroutine;
    private Coroutine _onHpBarCoroutine;

    private bool _takeDamage = false;

    #endregion

    #region Properties

    private float FillAmountDifference => (_hpBarDamage.fillAmount - _hpBar.fillAmount) / 30.0f;

    #endregion

    #region Initialize

    public void SetHpBar(EnemyType enemyType)
    {
        if (enemyType == EnemyType.Normal)
        {
            _hpBackGround.rectTransform.sizeDelta = _normalSize;
            UICanvas.enabled = false;
            _hpBarText.enabled = false;
        }
        if (enemyType == EnemyType.Boss)
        {
            _hpBackGround.rectTransform.sizeDelta = _bossSize;
            UICanvas.enabled = true;
            _hpBarText.enabled = true;
        }
    }

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetUI<Image>();
        _hpBackGround = GetUI<Image>("HpBackGround");
        _hpBar = GetUI<Image>("HpBar");
        _hpBarDamage = GetUI<Image>("HpBarDamage");

        SetUI<TextMeshProUGUI>();
        _hpBarText = GetUI<TextMeshProUGUI>("CurrentHP");
    }

    #endregion

    #region Event Method

    public void SetHealthBar(float currentHpPercent, long currentHp, bool isInit = false)
    {
        _hpBar.fillAmount = Mathf.Clamp(currentHpPercent, 0, 1);
        _hpBarText.text = currentHp.ToString();

        if (isInit) return;

        if (!UICanvas.enabled)
            UICanvas.enabled = true;

        if (this.gameObject.activeSelf)
        {
            if (_damageEffectCoroutine != null)
            {
                StopCoroutine(_damageEffectCoroutine);
                _damageEffectCoroutine = null;
            }
            _damageEffectCoroutine = StartCoroutine(DamageEffect());

            if (_onHpBarCoroutine != null)
            {
                StopCoroutine(_onHpBarCoroutine);
                _onHpBarCoroutine = null;
            }
            _onHpBarCoroutine = StartCoroutine(OnHpBar());
        }
    }

    public void ClearHpBar()
    {
        UICanvas.enabled = false;
        _hpBar.fillAmount = 1;
        _hpBarDamage.fillAmount = 1;

        if (_damageEffectCoroutine != null)
            _damageEffectCoroutine = null;
        if (_onHpBarCoroutine != null)
            _onHpBarCoroutine = null;
    }

    #endregion

    #region Event Coroutine

    IEnumerator OnHpBar()
    {
        yield return new WaitForSeconds(4.0f);
        UICanvas.enabled = false;
        _onHpBarCoroutine = null;
    }

    IEnumerator DamageEffect()
    {
        float decreasePerFrame = FillAmountDifference;

        while (_hpBar.fillAmount + decreasePerFrame < _hpBarDamage.fillAmount)
        {
            _hpBarDamage.fillAmount -= decreasePerFrame;
            yield return null;
        }

        if (_hpBar.fillAmount < _hpBarDamage.fillAmount)
        {
            _hpBarDamage.fillAmount = _hpBar.fillAmount;
        }
        _damageEffectCoroutine = null;
    }

    #endregion
}
