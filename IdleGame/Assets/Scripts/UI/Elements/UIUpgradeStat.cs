using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIUpgradeStat : MonoBehaviour
{
    #region Serialize Field

    [SerializeField] private TextMeshProUGUI textStatLevel;
    [SerializeField] private TextMeshProUGUI textStatValue;
    [SerializeField] private TextMeshProUGUI textUpdateCost;
    [SerializeField] private Button btnUpgradeStat;
    [SerializeField] private GameObject maxStat;
    [SerializeField] private GameObject dimScreen;
    [SerializeField] private QuestType questType;

    #endregion

    #region Field

    private Player player;
    private StatInfo statInfo;
    private UISceneMain uiSceneMain;

    private bool isPointerDown = false;
    private bool isHoldPressed = false;
    private bool isUpgradeRoutine = false;
    private DateTime pressTime;
    private float upgradeDelay = 0.05f;
    private bool isMax = false;

    #endregion

    #region Unity Flow

    private void Update()
    {
        if (isHoldPressed && !isUpgradeRoutine)
        {
            StartCoroutine(UpgradeRoutine());
        }
    }

    #endregion

    #region Upgrade Methods

    public void SetUpgradeStat(StatInfo statInfo, UISceneMain uiSceneMain)
    {
        this.player = Manager.Game.Player;
        this.statInfo = statInfo;
        this.uiSceneMain = uiSceneMain;

        UpdateText();

        btnUpgradeStat.gameObject.SetEvent(UIEventType.PointerDown, OnPointerDown);
        btnUpgradeStat.gameObject.SetEvent(UIEventType.PointerUp, OnPointerUp);
    }

    public void UpdateText()
    {
        // 레벨 최대치 도달할 경우, 버튼 잠김
        if (statInfo.MaxLevel != -1 && statInfo.Level >= statInfo.MaxLevel)
        {
            isMax = true;
            textStatLevel.text = "MAX";
            textStatValue.text = statInfo.GetString();

            btnUpgradeStat.gameObject.SetActive(false);
            maxStat.SetActive(true);

            uiSceneMain.UpdateStatLayoutChange(gameObject);
        }
        else
        {
            textStatLevel.text = $"Lv. {statInfo.Level}";
            textStatValue.text = statInfo.GetString();
            textUpdateCost.text = Utilities.ConvertToString(statInfo.UpgradeCost);
        }
    }

    public void TradeCheck()
    {
        if (isMax) return;

        if (player.Gold < statInfo.UpgradeCost)
        {
            dimScreen.SetActive(true);
        }
        else
        {
            dimScreen.SetActive(false);
        }
    }

    /// <summary>
    /// 플레이어 재화로 스탯 업그레이드
    /// </summary>
    public void UpdateUpgradeStat()
    {
        if (player.IsTradeGold(statInfo.UpgradeCost))
        {
            AudioSFX.Instance.PlayOneShot(Manager.Asset.GetAudio("testclick"));

            statInfo.AddModifier();
            UpdateQuestObjective();
            UpdateText();

            uiSceneMain.UpdatePlayerPower();
        }
    }

    private void UpdateQuestObjective()
    {
        if (questType == QuestType.DamageUp)
        {
            Manager.Quest.QuestDB[0].currentValue++;
            uiSceneMain.UpdateQuestObjective();
        }
        else if(questType == QuestType.HPUp)
        {
            Manager.Quest.QuestDB[1].currentValue++;
            uiSceneMain.UpdateQuestObjective();
        }
    }

    #endregion

    #region Button Methods

    private IEnumerator HoldPressListener()
    {
        while (isPointerDown)
        {
            double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

            if (elapsedSeconds >= 0.25f && !isHoldPressed)
            {
                isHoldPressed = true;
            }
            else if (elapsedSeconds >= 2.5f && isHoldPressed)
            {
                upgradeDelay = 0.01f;
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator UpgradeRoutine()
    {
        isUpgradeRoutine = true;

        UpdateUpgradeStat();
        yield return new WaitForSeconds(upgradeDelay);

        isUpgradeRoutine = false;
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pressTime = DateTime.Now;
        StartCoroutine(HoldPressListener());
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        if (!isHoldPressed) UpdateUpgradeStat();

        isPointerDown = false;
        isHoldPressed = false;
        upgradeDelay = 0.05f;
    }

    #endregion
}
