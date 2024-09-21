using UnityEngine;

public class MoveCommand: Command 
{
    private Vector3 direction;

    public MoveCommand(Vector2 direction) {
        this.direction = direction;
    }
    
    public void Do(PlayerController controller) {
        controller.transform.position += direction;
    }

    public void Undo(PlayerController controller) {
        controller.transform.position -= direction;
    }
} 