
public class DiscardCommand: Command {
    private Portable discarded;
    private Command lastCommand;

    public DiscardCommand(Portable discarded, PlayerController controller) {
        this.discarded = discarded;
        lastCommand = controller.LastCommand;
    }

    protected override void DoInner(PlayerController controller) {
        controller.HandleDiscard(discarded);
    }

    protected override void UndoInner(PlayerController controller) {
        controller.HandleRecover(discarded);
        lastCommand.Undo(controller);
    }
}