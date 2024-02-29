using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : UIBase
{
    #region Serialize Fields

    [SerializeField] private Image hpBar;
    [SerializeField] private Canvas UICanvas;

    #endregion

    #region Fields

    private Image _hpBackGround;
    private Image _hpBar;
    private Image _hpBarDamage;

    private Coroutine _damageEffectCoroutine;
    private Coroutine _onHpBarCoroutine;

    private bool _isDead = false;
    private float _decreasePerFrame;
    private float _timer;

    #endregion

    #region Properties

    private float FillAmountDifference => (_hpBarDamage.fillAmount - _hpBar.fillAmount) / 30.0f;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetUI<Image>();
        _hpBackGround = GetUI<Image>("Hp BackGround");
        _hpBar = GetUI<Image>("Hp Fill");
        _hpBarDamage = GetUI<Image>("Hp Empty");
    }

    #endregion

    public void SetGoldAmount()
    {
        Manager.UI.Top.UpdateGold();
        var mainScene = Manager.UI.CurrentScene as UISceneMain;
        mainScene.UpdateStatTradeCheck();
    }

    public void SetGemsAmout()
    {
        Manager.UI.Top.UpdateGems();
    }

    public void SetDamageFloating(Vector3 position, long Damage)
    {
        GameObject DamageHUD = Manager.ObjectPool.GetGo("Canvas_FloatingDamage");
        DamageHUD.GetComponent<UIFloatingText>().Initialize();
        DamageHUD.transform.position = gameObject.transform.position + position;
        DamageHUD.GetComponent<UIFloatingText>().SetDamage(Damage);
    }

    #region Event Method

    public void SetHealthBar(float currentHpPercent, long currentHp, bool isInit = false)
    {
        _hpBar.fillAmount = Mathf.Clamp(currentHpPercent, 0, 1);

        if (isInit) return;

        if (!UICanvas.enabled)
            UICanvas.enabled = true;

        if (!_isDead)
        {
            if (FillAmountDifference > _decreasePerFrame)
                _decreasePerFrame = FillAmountDifference;

            _timer = 4.0f;

            if (_damageEffectCoroutine == null)
            {
                _damageEffectCoroutine = StartCoroutine(DamageEffect());
            }

            if (_onHpBarCoroutine == null)
            {
                _onHpBarCoroutine = StartCoroutine(OnHpBar());
            }
        }
    }

    public void ClearHpBar()
    {
        UICanvas.enabled = false;
        _hpBar.fillAmount = 1;
        _hpBarDamage.fillAmount = 1;

        if (_damageEffectCoroutine != null)
        {
            StopCoroutine(_damageEffectCoroutine);
            _damageEffectCoroutine = null;
        }

        if (_onHpBarCoroutine != null)
        {
            StopCoroutine(_onHpBarCoroutine);
            _onHpBarCoroutine = null;
        }

        _decreasePerFrame = 0f;
        _timer = 0f;

        _isDead = true;
    }

    #endregion

    #region Event Coroutine

    IEnumerator OnHpBar()
    {
        while (_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }

        UICanvas.enabled = false;
        _onHpBarCoroutine = null;
        yield break;
    }

    IEnumerator DamageEffect()
    {
        while (_hpBar.fillAmount + _decreasePerFrame < _hpBarDamage.fillAmount)
        {
            _hpBarDamage.fillAmount -= _decreasePerFrame;
            yield return null;
        }

        if (_hpBar.fillAmount < _hpBarDamage.fillAmount)
        {
            _hpBarDamage.fillAmount = _hpBar.fillAmount;
        }
        _decreasePerFrame = 0f;
        _damageEffectCoroutine = null;
        yield break;
    }

    #endregion
}
