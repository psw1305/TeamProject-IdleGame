using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupSettings : UIPopup
{
    #region Fields

    private Toggle bgmToggle;
    private Toggle sfxToggle;
    private TextMeshProUGUI bgmText;
    private TextMeshProUGUI sfxText;

    #endregion

    #region Initialize

    protected override void Init()
    {
        base.Init();
        SetToggles();
        SetTexts();
        SetButtonEvents();

        SetAudio();
    }

    private void SetToggles()
    {
        SetUI<Toggle>();
        bgmToggle = GetUI<Toggle>("Toggle_BGM");
        sfxToggle = GetUI<Toggle>("Toggle_SFX");
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
        bgmText = GetUI<TextMeshProUGUI>("Txt_BGM");
        sfxText = GetUI<TextMeshProUGUI>("Txt_SFX");
    }

    private void SetButtonEvents()
    {
        SetUI<Button>();
        SetButtonEvent("DimScreen", UIEventType.Click, ClosePopup);
        SetButtonEvent("Btn_Close", UIEventType.Click, ClosePopup);
        SetButtonEvent("Btn_Nickname_Change", UIEventType.Click, OnNicknameChange);
    }

    private void SetAudio()
    {
        bgmToggle.onValueChanged.AddListener(OnChangedBGM);
        sfxToggle.onValueChanged.AddListener(OnChangedSFX);

        if (PlayerPrefs.GetInt("BGM", 1) == 1)
        {
            bgmToggle.isOn = true;
        }
        else
        {
            bgmToggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("SFX", 1) == 1)
        {
            sfxToggle.isOn = true;
        }
        else
        {
            sfxToggle.isOn = false;
        }
    }

    #endregion

    #region Button Events

    private void OnChangedBGM(bool isOn)
    {
        if (isOn)
        {
            bgmText.text = "켜기";
            AudioBGM.Instance.VolumeScale = 0.1f;
            PlayerPrefs.SetInt("BGM", 1);
        }
        else
        {
            bgmText.text = "끄기";
            AudioBGM.Instance.VolumeScale = 0.0f;
            PlayerPrefs.SetInt("BGM", 0);
        }
    }

    private void OnChangedSFX(bool isOn)
    {
        if (isOn)
        {
            sfxText.text = "켜기";
            AudioSFX.Instance.VolumeScale = 1.0f;
            PlayerPrefs.SetInt("SFX", 1);
        }
        else
        {
            sfxText.text = "끄기";
            AudioSFX.Instance.VolumeScale = 0.0f;
            PlayerPrefs.SetInt("SFX", 0);
        }
    }

    private void OnNicknameChange(PointerEventData eventData)
    {
        Manager.UI.ShowPopup<UIPopupChangeNickname>();
    }

    private void ClosePopup(PointerEventData eventData)
    {
        Manager.UI.ClosePopup();
    }

    #endregion
}
