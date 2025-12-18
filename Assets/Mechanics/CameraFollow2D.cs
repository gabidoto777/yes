using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos = -4.5f;
    [SerializeField] private float maxXPos = 233.3f;

    [SerializeField] private Transform target;

    // ====== CAMERA SHAKE FIELDS ======
    private float shakeTimeRemaining = 0f;
    private float shakeMagnitude = 0.3f;

    void Awake()
    {
        GameManager.Instance.OnPlayerSpawned += UpdatePlayerRef;

        if (!target)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                Debug.LogError("CameraFollow: No GameObject with tag 'Player' found in the scene.");
                return;
            }
            target = player.transform;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned -= UpdatePlayerRef;
        }
    }

    private void UpdatePlayerRef(controller playerInstance)
    {
        target = playerInstance.transform;
    }

    // 🔥 Call this from your player when you want a shake
    public void Shake(float duration, float magnitude)
    {
        shakeTimeRemaining = duration;
        shakeMagnitude = magnitude;
    }

    void Update()
    {
        if (!target) return;

        // Base follow (your original logic)
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);

        // Add shake offset on top
        if (shakeTimeRemaining > 0f)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float offsetX = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            pos += new Vector3(offsetX, offsetY, 0f);
        }

        transform.position = pos;
    }
}
