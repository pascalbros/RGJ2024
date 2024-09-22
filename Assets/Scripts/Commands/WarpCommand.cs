
public class WarpCommand: Command {
    private Vector3 destination;
    private Command lastCommand;

    public WarpCommand(Vector3 destination, PlayerController controller) {
        lastCommand = controller.LastCommand;
        this.destination = destination;        
    }

    protected override void DoInner(PlayerController controller) {
        controller.ApplyReflection(direction);
    }

    protected override void UndoInner(PlayerController controller) {
        controller.ApplyReflection(direction);
        lastCommand.Undo(controller);
    }
}
