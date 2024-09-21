using UnityEngine;

public class RotateCommand: Command {
    private Vector3 rotation;

    public RotateCommand(bool clockwise) {
        if (clockwise)
            rotation = new Vector3(0, 0, 90);
        else
            rotation = new Vector3(0, 0, -90);
    }

    public void Do(PlayerController controller) {
        controller.transform.Rotate(rotation);
    }

    public void Undo(PlayerController controller) {
        controller.transform.Rotate(-rotation);
    }
}