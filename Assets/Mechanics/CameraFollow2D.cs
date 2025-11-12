using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour

{

    public Transform target;

    public Vector3 offset = new Vector3(0, 0, -10);

    public float smoothSpeed = 50f;


    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = target.position + offset;

    }

}
