using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerRoot;   
    [SerializeField] private Transform cameraPivot;  

    [Header("Settings")]
    [SerializeField] private float sensitivity = 0.12f;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 70f;

    private InputSystem_Actions actions;
    private float pitch;

    private void OnEnable()
    {
        if (actions == null)
            actions = new InputSystem_Actions();

        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }

    private void Update()
    {
        Vector2 look = actions.Player.Look.ReadValue<Vector2>();

        float yaw = look.x * sensitivity;
        float pitchDelta = -look.y * sensitivity; 


        playerRoot.Rotate(0f, yaw, 0f, Space.World);


        pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);
        cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
