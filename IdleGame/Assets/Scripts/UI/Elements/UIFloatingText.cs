using TMPro;
using UnityEngine;

public class UIFloatingText : ObjectPoolable
{
    #region Fields

    private float _floatingSpeed = 0.25f;
    private float _alphaSpeed = 7.5f;
    private bool _isReleased = false;
    
    private TextMeshProUGUI _txtDamage;
    public Color Alpha;

    #endregion

    #region Properties

    public long damage { get; private set; }

    #endregion

    #region Init

    public void Initialize()
    {
        _txtDamage = GetComponentInChildren<TextMeshProUGUI>();
        //_txtDamage = GetComponent<TextMeshProUGUI>();
        Alpha = _txtDamage.color;
        Alpha = Color.white;
        Alpha.a = 1;
        //_txtDamage.text =string.Empty;
    }

    #endregion

    #region Unity Flow

    private void Update()
    {
        if (_isReleased)
            return;

        transform.Translate(Vector2.up * _floatingSpeed * Time.deltaTime );
        Alpha.a = Mathf.Lerp(Alpha.a, 0, Time.deltaTime * _alphaSpeed);
        
        _txtDamage.color = Alpha;
        if (Alpha.a <= 0.01)
        {
            ReleaseObject();
            _isReleased = true;
        }
    }

    #endregion

    #region SetDamage

    public void SetDamage(long value)
    {
        _isReleased = false;
        damage = value;
        _txtDamage.text = Utilities.ConvertToString(damage);
    }

    #endregion
}
