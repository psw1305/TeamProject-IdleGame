using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [HideInInspector]
    public int Damage;
    public GameObject ProjectileVFX;

    private void Start()
    {
        if (ProjectileVFX != null)
        {
            Instantiate(ProjectileVFX, transform.position, Quaternion.identity, gameObject.transform);
        }
        gameObject.AddComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, Manager.Game.Player.transform.position, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerModelTest")
        {
            Debug.Log(Damage);
            Destroy(gameObject);
        }
    }
}
