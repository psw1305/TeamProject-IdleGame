using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Image hpBar;
    [SerializeField] private Canvas UICanvas;

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

    public void SetGemsAmout()
    {
        var mainUI = Manager.UI.CurrentScene as UISceneMain;
        mainUI.UpdateGems();
    }

    public void SetDamageFloating(Vector3 position, long Damage)
    {
        GameObject DamageHUD = Manager.ObjectPool.GetGo("Canvas_FloatingDamage");
        //GameObject DamageHUD = Manager.ObjectPool.GetGo("FloatingText");
        //DamageHUD.transform.SetParent(UICanvas.transform, true);
        DamageHUD.GetComponent<UIFloatingText>().Initialize();
        DamageHUD.transform.position = gameObject.transform.position + position;
        DamageHUD.GetComponent<UIFloatingText>().SetDamage(Damage);
    }
}
