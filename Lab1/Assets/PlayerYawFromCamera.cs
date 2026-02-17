using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerYawFromCamera : MonoBehaviour
{
    [SerializeField] private CameraLook cameraLook;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        if (!cameraLook) return;


        Quaternion target = Quaternion.Euler(0f, cameraLook.Yaw, 0f);
        rb.MoveRotation(target);

        rb.angularVelocity = Vector3.zero;
    }
}
