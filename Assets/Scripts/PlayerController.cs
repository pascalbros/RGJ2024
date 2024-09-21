using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputHandler inputHandler;

    public Portable TopPortable { get; private set; }
    public Portable BottomPortable { get; private set; }

    Command lastCommand;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        inputHandler.OnMove += HandleMove;
        inputHandler.OnUndo += HandleUndo;
        inputHandler.OnAction += HandleAction;
    }
    private void OnDisable()
    {
        inputHandler.OnMove -= HandleMove;
        inputHandler.OnUndo -= HandleUndo;
        inputHandler.OnAction -= HandleAction;
    }

    void HandleMove(Vector2 input)
    {
        input = transform.InverseTransformDirection(input);
        Command topCommand = TopPortable.CanMove(input);
        Command bottomCommand = BottomPortable.CanMove(input);
        
        if (topCommand != null)
        {
            topCommand.Do(this);
            lastCommand = topCommand;
        }
        else if (bottomCommand != null)
        {
            bottomCommand.Do(this);
        }
    }
    void HandleAction()
    {
        Command topCommand = TopPortable.GetAction();
        Command bottomCommand = BottomPortable.GetAction();

        if (topCommand != null)
        {
            topCommand.Do(this);
            lastCommand = topCommand;
        }
        else if (bottomCommand != null)
        {
            bottomCommand.Do(this);
        }
    }
    void HandleUndo()
    {
        if (lastCommand == null)
            return;
        lastCommand.Undo(this);
        lastCommand = null;
    }
}
