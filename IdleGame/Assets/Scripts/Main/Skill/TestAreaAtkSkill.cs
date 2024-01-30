using System.Collections;
using UnityEngine;

public class TestAreaAtkSkill : BaseSkill
{
    [SerializeField] private GameObject projectileSpawnArea;

    private GameObject projectile;

    [SerializeField] Vector2 minDestinationPosition;
    [SerializeField] Vector2 maxDestinationPosition;

    [SerializeField] int AtkCount;

    private Coroutine AtkCor;
    protected override void ApplySkillEffect()
    {
        AtkCor = StartCoroutine(AtkLoop());
    }
    protected override void RemoveSkillEffect()
    {
        StopCoroutine(AtkCor);
        AtkCor = null;
    }

    IEnumerator AtkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            projectile = Manager.Resource.InstantiatePrefab("TestProjectile");
            projectile.transform.position = new Vector2(0, 5);

            Manager.Game.Player.FinalAttackDamage(out projectile.GetComponent<TestAreaAtkSkillProjectile>().Damage
                , out projectile.GetComponent<TestAreaAtkSkillProjectile>().DamageTypeValue);

            projectile.GetComponent<TestAreaAtkSkillProjectile>().TargetPosition = new Vector2(Random.Range(minDestinationPosition.x, maxDestinationPosition.x), Random.Range(minDestinationPosition.y, maxDestinationPosition.y));
        }
    }
}
