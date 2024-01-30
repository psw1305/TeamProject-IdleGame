using TMPro;
using UnityEngine;

public class UIFloatingText : ObjectPoolable
{
    private float _floatingSpeed = 0.25f;
    private float _alphaSpeed = 7.5f;
    private float _destroyTime = 1.0f;

    private bool _isReleased = false;
    
    private TextMeshProUGUI _txtDamage;
    public Color Alpha;

    public long damage { get; private set; }

    public void Initialize()
    {
        _txtDamage = GetComponentInChildren<TextMeshProUGUI>();
        Alpha = _txtDamage.color;
        Alpha.a = 1;
        _txtDamage.text = damage.ToString();
    }

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

    public void SetDamage(long value)
    {
        _isReleased = false;
        damage = value;
        _txtDamage.text = damage.ToString();
    }
}
