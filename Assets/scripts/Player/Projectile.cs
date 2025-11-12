using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.5f, 10.0f)] private float lifeTime = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Destroy(gameObject, lifeTime);

    public void SetVelocity(Vector2 velocity) => GetComponent<Rigidbody2D>().linearVelocity = velocity;

}

