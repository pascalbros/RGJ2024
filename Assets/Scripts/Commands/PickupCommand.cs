using UnityEngine;

public class PickupCommand : Command
{
    private bool atTop;
    private Portable pickedUp;
    private Portable oldTop;
    private Portable oldBottom;
    private Command lastCommand;

    public PickupCommand(Portable pickedUp, bool atTop, PlayerController controller) {
        this.pickedUp = pickedUp;
        lastCommand = controller.LastCommand;
        this.atTop = atTop; 
        if (atTop)
            oldTop = controller.TopPortable;
        else
            oldBottom = controller.BottomPortable;
    }

    protected override void DoInner(PlayerController controller) {
        if (atTop) 
            controller.TopPortable = pickedUp;
        else
            controller.BottomPortable = pickedUp;
    }

    protected override void UndoInner(PlayerController controller) {
        if (atTop) {
            //TODO drop old top
            Debug.Log("Drop top", controller.TopPortable);
            controller.TopPortable = oldTop;
        } else {
            //TODO drop old bottom
            Debug.Log("Drop bottom", controller.BottomPortable);
            controller.BottomPortable = oldBottom;
        }
        lastCommand.Undo(controller);
    }
}
