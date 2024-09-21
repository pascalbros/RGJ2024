using UnityEngine;

public class RotateCommand: Command {
    private Vector3 rotation;

    public RotateCommand(Portable source, bool clockwise) {
        this.source = source;
        if (clockwise)
            rotation = new Vector3(0, 0, -90);
        else
            rotation = new Vector3(0, 0, 90);
    }

    protected override void DoInner(PlayerController controller) {
        controller.ApplyRotation(rotation);
    }

    protected override void UndoInner(PlayerController controller) {
        controller.ApplyRotation(-rotation);
    }
}