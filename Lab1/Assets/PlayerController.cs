using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform yawSource;   // YawPivot 或 Main Camera
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float inputDeadzone = 0.02f;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 6.5f;   // 跳的力度（调大=跳高）
    [SerializeField] private LayerMask groundMask;        // 地面层
    [SerializeField] private float groundCheckDistance = 0.15f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpQueued;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
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

    private void OnMove(Vector2 v)
    {
        if (v.sqrMagnitude < inputDeadzone * inputDeadzone) v = Vector2.zero;
        moveInput = v;
    }

    private void OnJump()
    {
        jumpQueued = true;
    }

    private void FixedUpdate()
    {
        // ---- MOVE (XZ) ----
        if (yawSource)
        {
            Vector3 forward = yawSource.forward; forward.y = 0f;
            Vector3 right = yawSource.right; right.y = 0f;

            if (forward.sqrMagnitude > 0f) forward.Normalize();
            if (right.sqrMagnitude > 0f) right.Normalize();

            Vector3 move = (right * moveInput.x + forward * moveInput.y);
            if (move.sqrMagnitude > 1f) move.Normalize();

            Vector3 delta = move * moveSpeed * Time.fixedDeltaTime;

            // 只移动XZ，不改Y
            rb.MovePosition(rb.position + new Vector3(delta.x, 0f, delta.z));
        }

        // ---- JUMP ----
        if (jumpQueued)
        {
            jumpQueued = false;

            if (IsGrounded())
            {
                // 直接设置y速度最稳定（不被MovePosition覆盖）
                Vector3 v = rb.linearVelocity;
                rb.linearVelocity = new Vector3(v.x, jumpVelocity, v.z);
            }
        }
    }

    private bool IsGrounded()
    {
        // 从碰撞体底部向下打一条短射线
        Collider col = GetComponent<Collider>();
        Vector3 origin = col.bounds.center;
        origin.y = col.bounds.min.y + 0.02f;

        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore);
    }
}
