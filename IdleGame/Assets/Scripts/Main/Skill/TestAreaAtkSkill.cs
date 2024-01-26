using System.Collections;
using UnityEngine;

public class TestAreaAtkSkill : BaseSkill, ISkillUsingEffect
{
    [SerializeField] private GameObject projectileSpawnArea;

    private GameObject projectile;

    [SerializeField] Vector2 minDestinationPosition;
    [SerializeField] Vector2 maxDestinationPosition;

    [SerializeField] int AtkCount;

    private Coroutine AtkCor;
    public void ApplySkillEffect()
    {
        AtkCor = StartCoroutine(AtkLoop());
    }
    public void RemoveSkillEffect()
    {
        StopCoroutine(AtkCor);
        AtkCor = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Manager.Game.Player.enemyList.Count > 0)
            {
                UseSkill(this);
            }
        }
    }

    IEnumerator AtkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            projectile = Manager.Resource.InstantiatePrefab("TestProjectile");
            projectile.transform.position = new Vector2(0, 5);
            projectile.GetComponent<TestAreaAtkSkillProjectile>().Damage = 1;
            projectile.GetComponent<TestAreaAtkSkillProjectile>().TargetPosition = new Vector2(Random.Range(minDestinationPosition.x, maxDestinationPosition.x), Random.Range(minDestinationPosition.y, maxDestinationPosition.y));
        }
    }


}
