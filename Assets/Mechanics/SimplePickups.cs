using JetBrains.Annotations;
using UnityEngine;

public class SimplePickups : MonoBehaviour
{

    public enum pickupType
    {
        Life,
        Powerup
    }

    public pickupType typeOfPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (typeOfPickup)
            {
                case pickupType.Life:
                    // Implement life pickup logic
                    Debug.Log("Life picked up!");
                    break;
                case pickupType.Powerup:
                    // Implement powerup pickup logic
                    Debug.Log("Powerup picked up!");
                    break;
            }
            // Destroy the pickup after being collected
            Destroy(gameObject);
        }
    }

}
