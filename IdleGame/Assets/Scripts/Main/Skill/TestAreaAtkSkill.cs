using System.Collections;
using UnityEngine;

public class TestAreaAtkSkill : BaseSkill
{
    [SerializeField] private GameObject projectileSpawnArea;


    [SerializeField] private Vector2 minDestinationPosition;
    [SerializeField] private Vector2 maxDestinationPosition;

    private GameObject _projectile;
    private Coroutine _atkCor;

    protected override void ApplySkillEffect()
    {
        _atkCor = StartCoroutine(AtkLoop());
    }

    protected override void RemoveSkillEffect()
    {
        StopCoroutine(_atkCor);
        _atkCor = null;
    }

    IEnumerator AtkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            _projectile = Manager.Resource.InstantiatePrefab("TestProjectile");
            _projectile.transform.position = new Vector2(0, 5);

            Manager.Game.Player.FinalAttackDamage(out _projectile.GetComponent<TestAreaAtkSkillProjectile>().Damage
                , out _projectile.GetComponent<TestAreaAtkSkillProjectile>().DamageTypeValue);

            _projectile.GetComponent<TestAreaAtkSkillProjectile>().TargetPosition = new Vector2(Random.Range(minDestinationPosition.x, maxDestinationPosition.x), Random.Range(minDestinationPosition.y, maxDestinationPosition.y));
        }
    }
}
