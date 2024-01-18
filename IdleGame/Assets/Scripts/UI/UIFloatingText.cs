using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFloatingText : MonoBehaviour
{
    private float _floatingSpeed;
    private float _alphaSpeed;
    private float _destroyTime;
    
    private TextMeshProUGUI _txtDamage;
    private Color alpha;

    public long damage { get; private set; }

    private void Start()
    {
        _floatingSpeed = 0.25f;
        _alphaSpeed = 7.5f;
        _destroyTime = 1.0f;

        _txtDamage = GetComponent<TextMeshProUGUI>();
        alpha = _txtDamage.color;
        _txtDamage.text = damage.ToString();

        Invoke("DestroyObject", _destroyTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * _floatingSpeed * Time.deltaTime );
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * _alphaSpeed);
        _txtDamage.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(transform.root.gameObject);
    }

    public void SetDamage(long value)
    {
        damage = value;
    }
}
