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

        // ✅ Player 永远对齐相机 yaw（水平）
        Quaternion target = Quaternion.Euler(0f, cameraLook.Yaw, 0f);
        rb.MoveRotation(target);

        rb.angularVelocity = Vector3.zero;
    }
}
