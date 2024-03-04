using UnityEngine;
using UnityEngine.UI;

public class UINPCPortrait : MonoBehaviour
{
    [SerializeField] private Sprite[] _body;
    [SerializeField] private Sprite[] _face;
    [SerializeField] private Sprite[] _arm;

    [SerializeField] private Image[] _portrait;

    public void SpriteChange(int body, int face, int arm)
    {
        if(body == 1)
        {
            _portrait[2].gameObject.SetActive(true);
        }
        else if(body == 0) 
        {
            _portrait[2].gameObject.SetActive(false);
        }

        _portrait[0].sprite = _body[body];
        _portrait[1].sprite = _face[face];
        _portrait[2].sprite = _arm[arm];
    }
}
