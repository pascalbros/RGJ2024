using UnityEngine;

public class PlayerController: MonoBehaviour {
    private enum State { GAME, BUSY, PICKUP }

    State state = State.GAME;
    Portable watinigForSelection;

    private Vector3 movementMask = new Vector3(1, 1, 0);

    InputHandler inputHandler;
    private InventoryManager inventory;

    public Portable TopPortable { get => inventory.top; set => inventory.SetTop(value); }
    public Portable BottomPortable { get => inventory.bottom; set => inventory.SetBottom(value); }

    public Command LastCommand { get; private set; }

    // last player input in global space
    public Vector2 LastMovement { get; private set; }

    private void Awake() {
        inputHandler = GetComponent<InputHandler>();
        inventory = GetComponent<InventoryManager>();
    }

    private void OnEnable() {
        inputHandler.OnMove += HandleMove;
        inputHandler.OnUndo += HandleUndo;
        inputHandler.OnAction += HandleAction;
        inputHandler.OnSelectPickup += HandlePickupSelection;
    }
    private void OnDisable() {
        inputHandler.OnMove -= HandleMove;
        inputHandler.OnUndo -= HandleUndo;
        inputHandler.OnAction -= HandleAction;
        inputHandler.OnSelectPickup -= HandlePickupSelection;
    }

    public void InitPortables(Portable top, Portable bottom) {
        inventory.SetTop(top);
        inventory.SetBottom(bottom);
    }

    public void HandleMove(Vector2 input) {
        if (state != State.GAME) return;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, input, 1, LayerMask.GetMask("Wall"));
        if (hit.collider != null && hit.collider.gameObject.tag == "Wall")
            return;

        //start from top if top is not consumable or are both consumable
        bool startFromTop = !(TopPortable?.IsConsumable ?? false) || (BottomPortable?.IsConsumable ?? false);

        if (startFromTop && HandleMove(input, TopPortable)) return;
        if (HandleMove(input, BottomPortable)) return;
        if (!startFromTop && HandleMove(input, TopPortable)) return;
    }
    private bool HandleMove(Vector2 input, Portable portable) {
        if (portable == null) return false;

        MoveCommand cmd = portable.CanMove(input);
        if (cmd == null) return false;

        LastMovement = cmd.direction;
        LastCommand = cmd;
        cmd.Do(this);
        return true;
    }
    public void HandleAction() {
        if (state != State.GAME) return;
        Command topCommand = TopPortable?.GetAction();

        if (topCommand != null) {
            LastCommand = topCommand;
            topCommand.Do(this);
            return;
        }

        Command bottomCommand = BottomPortable?.GetAction();
        if (bottomCommand != null) {
            LastCommand = bottomCommand;
            bottomCommand.Do(this);
        }
    }
    public void HandleUndo() {
        if (state != State.GAME) return;
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
    public void HandleDiscard(Portable portable) => inventory.Discard(portable);
    public void HandleRecover(Portable portable) => inventory.Recover(portable);
    public void HandlePickupSelection(PickupSelection selection) {
        switch (selection) {
            case PickupSelection.TOP:
                HandlePickup(watinigForSelection, true);
                break;
            case PickupSelection.BOTTOM:
                HandlePickup(watinigForSelection, false);
                break;
            case PickupSelection.DISCARD:
                if (!watinigForSelection.IsKey) {
                    var cmd = new DiscardCommand(watinigForSelection, this);
                    cmd.Do(this);
                    LastCommand = cmd;
                }
                break;
        }
        watinigForSelection = null;
        state = State.GAME;
    }

    public void ApplyMovement(Vector3 delta) {
        transform.position += delta;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.4f, LayerMask.GetMask("Portable"));
        if (hit.collider != null) {
            Portable portable = hit.collider.transform.parent.GetComponent<Portable>();
            if (portable != null) {
                TryToPickup(portable);
            } else if (hit.collider.transform.TryGetComponent<ExitController>(out var exit)) {
                exit.OnExit();
            }
        }
    }

    public void ApplyRotation(Vector3 angle) {
        transform.Rotate(angle);
    }

    public void ApplyReflection(Vector2 direction) {
        Debug.Log("reflect " + direction);
        transform.Rotate(direction * 180);
    }

    public void TryToPickup(Portable portable) {
        if (TopPortable == null) {
            HandlePickup(portable, true);
            return;
        }
        if (BottomPortable == null) {
            HandlePickup(portable, false);
            return;
        }

        state = State.PICKUP;
        watinigForSelection = portable;

        //TODO

        //Selection selection = ShowSelectionMenu(
        //    TopPortable.bigIcon.Sprite,
        //    BottomPortable.bigIcon.Sprite,
        //    portable.bigIcon.Sprite,
        //    transform.rotation
        //);

    }
}
