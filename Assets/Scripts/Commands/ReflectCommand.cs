using UnityEngine;

public class ReflectCommand: Command {
    private Vector3 direction;

    public ReflectCommand(Vector2 direction) {
        this.direction = direction * 180;
    }

    public void Do(PlayerController controller) {
        controller.transform.Rotate(direction);
    }

    public void Undo(PlayerController controller) {
        Do(controller);
    }
}