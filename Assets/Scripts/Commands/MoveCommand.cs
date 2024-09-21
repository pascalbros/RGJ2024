using UnityEngine;

public class MoveCommand: Command 
{
    public Vector3 direction { get; private set; }

    public MoveCommand(Vector2 direction) {
        this.direction = direction;
    }
    
    public void Do(PlayerController controller) {
        controller.ApplyMovement(direction);
    }

    public void Undo(PlayerController controller) {
        controller.ApplyMovement(-direction);
    }
} 