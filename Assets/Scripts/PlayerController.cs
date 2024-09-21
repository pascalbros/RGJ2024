using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputHandler inputHandler;

    public Portable TopPortable { get; set; }
    public Portable BottomPortable { get; set; }

    public Command LastCommand { get; private set; }

    // last player input in global space
    public Vector2 LastMovement { get; private set; }

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
        var localInput = transform.InverseTransformDirection(input);
        Command topCommand = TopPortable?.CanMove(localInput);
        
        if (topCommand != null)
        {
            LastMovement = input;
            LastCommand = topCommand;
            topCommand.Do(this);
            return;
        }

        Command bottomCommand = BottomPortable?.CanMove(localInput);
        if (bottomCommand != null)
        {
            LastMovement = input;
            LastCommand = bottomCommand;
            bottomCommand.Do(this);
        }
    }
    void HandleAction()
    {
        Command topCommand = TopPortable?.GetAction();

        if (topCommand != null)
        {
            LastCommand = topCommand;
            topCommand.Do(this);
            return;
        }

        Command bottomCommand = BottomPortable?.GetAction();
        if (bottomCommand != null)
        {
            LastCommand = bottomCommand;
            bottomCommand.Do(this);
        }
    }
    void HandleUndo()
    {
        if (LastCommand == null)
            return;
        LastCommand.Undo(this);
        LastCommand = null;
    }
    public void HandleReflection() {
        var cmd = new ReflectCommand(this);
        cmd.Do(this);
        LastCommand = cmd;
    }

    public void ApplyMovement(Vector3 delta) {
        transform.position += delta;
    }

    public void ApplyRotation(Vector3 angle) {
        transform.Rotate(angle);
    }

    public void ApplyReflection() {
        transform.Rotate(new Vector3(0, 180, 0));
    }

}
