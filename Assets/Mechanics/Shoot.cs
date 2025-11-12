using UnityEngine;
using UnityEngine.Rendering;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Vector2 initialShotVelocity = Vector2.zero;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Projectile projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initialShotVelocity == Vector2.zero)
        {
            initialShotVelocity = new Vector2 (10, 0);
            Debug.LogWarning("Initial shot velocity not set on Shoot Component of " + gameObject.name + ", defaulting to " + initialShotVelocity);
        }

        if (spawnPointRight == null || spawnPointLeft == null || projectilePrefab == null)
        {
            Debug.LogWarning("Spawn points or projectile not set on Shoot Component of " + gameObject.name);
        }
    }

    public void Fire()
    {
        Projectile curProjectile;
        if (!sr.flipX)
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, Quaternion.identity);
            curProjectile.SetVelocity(initialShotVelocity);
        }
        else
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, Quaternion.identity);
            curProjectile.SetVelocity(initialShotVelocity);
        }
    }

}
