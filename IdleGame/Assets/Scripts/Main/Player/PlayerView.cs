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
        Manager.UI.Top.UpdateGold();
        var mainScene = Manager.UI.CurrentScene as UISceneMain;
        mainScene.UpdateStatTradeCheck();
    }

    public void SetGemsAmout()
    {
        Manager.UI.Top.UpdateGems();
    }

    public void SetDamageFloating(Vector3 position, long Damage)
    {
        GameObject DamageHUD = Manager.ObjectPool.GetGo("Canvas_FloatingDamage");
        DamageHUD.GetComponent<UIFloatingText>().Initialize();
        DamageHUD.transform.position = gameObject.transform.position + position;
        DamageHUD.GetComponent<UIFloatingText>().SetDamage(Damage);
    }
}
