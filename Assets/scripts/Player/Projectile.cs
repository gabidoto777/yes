using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType type = ProjectileType.Player;
    [SerializeField, Range(0.5f, 10)] private float lifetime = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Destroy(gameObject, lifetime);
    public void SetVelocity(Vector2 velocity) => GetComponent<Rigidbody2D>().linearVelocity = velocity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == ProjectileType.Player)
        {
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }
}

public enum ProjectileType
{
    Player,
    Enemy
}

