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
    [SerializeField] private QuestType questType;

    #endregion

    #region Field

    private Player player;
    private StatInfo statInfo;

    private bool isPointerDown = false;
    private bool isHoldPressed = false;
    private bool isUpgradeRoutine = false;
    private DateTime pressTime;
    private float upgradeDelay = 0.05f;

    #endregion

    public void SetUpgradeStat(StatInfo statInfo)
    {
        this.player = Manager.Game.Player;
        this.statInfo = statInfo;

        textStatLevel.text = $"Lv. {statInfo.Level}";
        textStatValue.text = statInfo.GetString();
        textUpdateCost.text = statInfo.UpgradeCost.ToString();

        btnUpgradeStat.gameObject.SetEvent(UIEventType.PointerDown, OnPointerDown);
        btnUpgradeStat.gameObject.SetEvent(UIEventType.PointerUp, OnPointerUp);
    }

    private void Update()
    {
        if (isHoldPressed && !isUpgradeRoutine)
        {
            StartCoroutine(UpgradeRoutine());
        }
    }

    public void UpdateUpgradeStat()
    {
        if (player.IsTradeGold(statInfo.UpgradeCost))
        {
            AudioSFX.Instance.PlayOneShot(Manager.Resource.GetAudio("14_item2"));

            statInfo.AddModifier();

            UpdateQuestObjective();

            textStatLevel.text = $"Lv. {statInfo.Level}";
            textStatValue.text = statInfo.GetString();
            textUpdateCost.text = statInfo.UpgradeCost.ToString();
        }
    }

    private void UpdateQuestObjective()
    {
        if (questType == QuestType.DamageUp)
        {
            Manager.Quest.QuestDB[0].currentValue++;
            UISceneMain uiSceneMain = Manager.UI.CurrentScene as UISceneMain;
            uiSceneMain.UpdateQuestObjective();
        }
        else if(questType == QuestType.HPUp)
        {
            Manager.Quest.QuestDB[1].currentValue++;
            UISceneMain uiSceneMain = Manager.UI.CurrentScene as UISceneMain;
            uiSceneMain.UpdateQuestObjective();
        }
    }

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
}
