using UnityEngine;
using System;

public class ReflectCommand: Command {
    private Vector2 direction;
    private Command lastCommand;

    public ReflectCommand(PlayerController controller) {
        lastCommand = controller.LastCommand;
        if (Math.Abs(controller.LastMovement.x) > Math.Abs(controller.LastMovement.y))
            direction = Vector2.up;
        else
            direction = Vector2.right;
        
    }

    public void Do(PlayerController controller) {
        controller.ApplyReflection(direction);
    }

    public void Undo(PlayerController controller) {
        controller.ApplyReflection(direction);
        lastCommand.Undo(controller);
    }
}