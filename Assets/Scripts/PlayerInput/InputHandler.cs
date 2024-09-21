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
        Debug.Log("awake ih");
        playerInput = GetComponent<PlayerInput>();
         Debug.Log("awaken ih "+playerInput);
   }

    private void OnEnable()
    {
        Debug.Log("enable ih "+playerInput);
        if(playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        Debug.Log("enabled ih "+playerInput);
        playerInput.onActionTriggered += OnActionTriggered;
    }
    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log(context);
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
        Vector2 value = context.ReadValue<Vector2>();
        if (Mathf.Abs(value.x) == Mathf.Abs(value.y))
        {
            value = Vector2.zero;
        }
        return value;
    }
}
