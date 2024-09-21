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

    public void Do(PlayerController controller) {
        if (atTop) 
            controller.TopPortable = pickedUp;
        else
            controller.BottomPortable = pickedUp;
    }

    public void Undo(PlayerController controller) {
        if (atTop) {
            //TODO drop old top
            controller.TopPortable = oldTop;
        } else {
            //TODO drop old bottom
            controller.BottomPortable = oldBottom;
        }
        lastCommand.Undo(controller);
    }
}
