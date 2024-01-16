using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupEquipItemInfo : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    [SerializeField] private Image typeIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;

    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI itemLevelText;

    [SerializeField] private TextMeshProUGUI itemHasCount;

    [SerializeField] private TextMeshProUGUI EquipEffect;
    [SerializeField] private TextMeshProUGUI RetentionEffect;

    private SlotData _selectItemData;

    public void SetSelectItemInfo(SlotData selectItemData)
    {
        //UI 정보를 세팅합니다.
        _selectItemData = selectItemData;
        itemNameText.text = _selectItemData.itemName;
        rarityText.text = _selectItemData.rarity;
        itemLevelText.text = _selectItemData.level.ToString();
        itemHasCount.text = _selectItemData.hasCount.ToString();
        EquipEffect.text =$"{_selectItemData.equipStat + _selectItemData.reinforceEquip * _selectItemData.level}%";
        RetentionEffect.text = $"{_selectItemData.retentionEffect + _selectItemData.reinforceEffect * _selectItemData.level}%";
    }


}
