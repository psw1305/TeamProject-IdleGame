using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINPCPortrait : MonoBehaviour
{
    [SerializeField] private Sprite[] body;
    [SerializeField] private Sprite[] face;
    [SerializeField] private Sprite[] arm;

    [SerializeField] private Image[] portrait;


    private Color On = new Color(1, 1, 1);
    private Color Off = new Color(0, 0, 0);


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            portrait[0].sprite = body[1];
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            portrait[1].sprite = face[1];
        }
        if (Input.GetKeyDown(KeyCode.C))
        {            
            portrait[2].sprite = arm[1];
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            portrait[2].gameObject.SetActive(!portrait[2].IsActive());
        }
    }
}
