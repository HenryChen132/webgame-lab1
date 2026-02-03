using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private Vector2 moveInput;
    private float verticalVelocity;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.JumpEvent += OnJump;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.JumpEvent -= OnJump;
    }

    private void OnMove(Vector2 v) => moveInput = v;

    private void OnJump()
    {
        if (controller != null && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void Update()
    {
        if (controller == null) return;

  
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 horizontal = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        Vector3 motion = horizontal + Vector3.up * verticalVelocity;

        controller.Move(motion * Time.deltaTime);
    }
}
