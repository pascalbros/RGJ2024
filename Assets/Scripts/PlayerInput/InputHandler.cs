using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnAction;
    public event Action OnUndo;

    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if(playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
    }
    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (context.action.name)
            {
                case "Move":
                    var value = FixMovementInput(context);
                    if (value != Vector2.zero)
                    OnMove?.Invoke(value);
                    break;
                case "Undo":
                    OnUndo?.Invoke();
                    break;
                case "Action":
                    OnAction?.Invoke();
                    break;
            }
        }
    }
    Vector2 FixMovementInput(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>().normalized;
        if (Mathf.Abs(value.x) == Mathf.Abs(value.y))
        {
            value = Vector2.zero;
        }
        return value;
    }
}
