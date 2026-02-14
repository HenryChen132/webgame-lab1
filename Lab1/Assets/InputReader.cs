using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject
{
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;

    private InputSystem_Actions actions;

    private void OnEnable()
    {
        if (actions == null)
        {
            actions = new InputSystem_Actions();

            actions.Player.Move.performed += ctx =>
                MoveEvent?.Invoke(ctx.ReadValue<Vector2>());

            actions.Player.Move.canceled += _ =>
                MoveEvent?.Invoke(Vector2.zero);

            // ✅ Jump：按下触发一次
            actions.Player.Jump.performed += _ =>
                JumpEvent?.Invoke();
        }

        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }
}
