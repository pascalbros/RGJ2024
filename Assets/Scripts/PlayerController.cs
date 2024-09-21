using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 movementMask = new Vector3(1,1,0);

    InputHandler inputHandler;
    private InventoryManager inventory;

    public Portable TopPortable { get => inventory.top; set => inventory.SetTop(value); }
    public Portable BottomPortable { get => inventory.bottom; set => inventory.SetBottom(value); }

    public Command LastCommand { get; private set; }

    // last player input in global space
    public Vector2 LastMovement { get; private set; }

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        inventory = GetComponent<InventoryManager>();
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

    public void HandleMove(Vector2 input)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, input, 1, LayerMask.GetMask("Wall"));
        Debug.Log("hit "+hit.collider?.gameObject?.tag, hit.collider);
        if (hit.collider != null && hit.collider.gameObject.tag == "Wall")
            return;

        var localInput = transform.InverseTransformDirection(input);

        //start from top if top is not consumable or are both consumable
        bool startFromTop = !(TopPortable?.IsConsumable ?? false) || (BottomPortable?.IsConsumable ?? false);

        if (startFromTop && HandleMove(localInput, TopPortable)) return;
        if (HandleMove(localInput, BottomPortable)) return;
        if (!startFromTop && HandleMove(localInput, TopPortable)) return;
    }
    private bool HandleMove(Vector2 input, Portable portable) {
        Command cmd = portable?.CanMove(input);
        if (cmd == null) return false;

        LastMovement = input;
        LastCommand = cmd;
        cmd.Do(this);
        return true;
    }
    public void HandleAction()
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
    public void HandleUndo()
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
    public void HandlePickup(Portable portable, bool atTop) {
        var cmd = new PickupCommand(portable, atTop, this);
        cmd.Do(this);
        LastCommand = cmd;
    }
    public void HandleConsumed(Portable portable) {
        if (TopPortable == portable)
            HandlePickup(null, true);
        else if (BottomPortable == portable)
            HandlePickup(null, false);
    }
    public void HandleDrop(bool top) => inventory.Drop(top);

    public void ApplyMovement(Vector3 delta) {
        transform.position += Vector3.Scale(transform.TransformDirection(delta), movementMask).normalized;
    }

    public void ApplyRotation(Vector3 angle) {
        transform.Rotate(angle);
    }

    public void ApplyReflection(Vector2 direction) {
        Debug.Log("reflect "+direction);
        transform.Rotate(direction * 180);
    }

}
