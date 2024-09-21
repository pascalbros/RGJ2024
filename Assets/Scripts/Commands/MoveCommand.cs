using UnityEngine;

public class MoveCommand: Command 
{
    public Vector3 direction { get; private set; }

    public MoveCommand(Portable source, Vector2 direction) {
        this.source = source;
        this.direction = direction;
    }
    
    protected override void DoInner(PlayerController controller) {
        controller.ApplyMovement(direction);
    }

    protected override void UndoInner(PlayerController controller) {
        controller.ApplyMovement(-direction);
    }
} 