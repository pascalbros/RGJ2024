using UnityEngine;
using System;

public class ReflectCommand: Command {
    private Vector3 direction;
    private Command lastCommand;

    public ReflectCommand(PlayerController controller) {
        lastCommand = controller.LastCommand;
        if (Math.Abs(controller.LastMovement.x) > Math.Abs(controller.LastMovement.y))
            direction = new Vector3(180, 0, 0);
        else
            direction = new Vector3(0, 180, 0);
    }

    public void Do(PlayerController controller) {
        controller.ApplyReflection();
    }

    public void Undo(PlayerController controller) {
        controller.ApplyReflection();
        lastCommand.Undo(controller);
    }
}