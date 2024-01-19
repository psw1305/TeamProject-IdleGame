using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Image hpBar;

    #endregion

    public void SetHealthBar(float currentHpPercent)
    {
        hpBar.fillAmount = Mathf.Clamp(currentHpPercent, 0, 1);
    }

    public void SetGoldAmount()
    {
        var mainUI = Manager.UI.CurrentScene as UISceneMain;
        mainUI.UpdateGold();
    }

    public void SetDamageFloating(Vector3 position, long Damage)
    {
        GameObject DamageHUD = Manager.Resource.InstantiatePrefab("Canvas_FloatingDamage");
        DamageHUD.transform.position = gameObject.transform.position + position;
        DamageHUD.GetComponentInChildren<UIFloatingText>().SetDamage(Damage);
    }
}
