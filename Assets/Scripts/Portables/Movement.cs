using UnityEngine;

public class Movement: Portable {
    public bool up;
    public bool right;
    public bool down;
    public bool left;

    public override MoveCommand CanMove(Vector2 movement)
    {
        if (!up && movement.y > 0.5) return null;
        if (!down && movement.y < -0.5) return null;
        if (!right && movement.x > 0.5) return null;
        if (!left && movement.x < -0.5) return null;

        return new MoveCommand(movement.normalized);
    }

}