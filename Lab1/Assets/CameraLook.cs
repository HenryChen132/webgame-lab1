using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pitchPivot; // Player cllid obj -- PitchPivot

    [Header("Settings")]
    [SerializeField] private float sensitivity = 0.08f;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Filter")]
    [SerializeField] private float mouseDeadzone = 0.02f;

    public float Yaw { get; private set; }   // for PlayerYawFromCamera 
    private float pitch;

    private void OnEnable()
    {
        
        pitch = Normalize(pitchPivot.localEulerAngles.x);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void LateUpdate()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        if (Mathf.Abs(delta.x) < mouseDeadzone) delta.x = 0f;
        if (Mathf.Abs(delta.y) < mouseDeadzone) delta.y = 0f;

    
        Yaw += delta.x * sensitivity;


        pitch -= delta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private static float Normalize(float a)
    {
        a %= 360f;
        if (a > 180f) a -= 360f;
        return a;
    }

    // UI adjustment for sensitivity
    public float Sensitivity
    {
        get => sensitivity;
        set => sensitivity = Mathf.Clamp(value, 0.01f, 2f);
    }
}
