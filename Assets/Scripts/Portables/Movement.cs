using UnityEngine;

public class Movement: Portable {
    public bool up;
    public bool right;
    public bool down;
    public bool left;

    public override MoveCommand CanMove(Vector2 movement)
    {
        var local = transform.InverseTransformDirection(movement);
        if (!up && local.y > 0.5) return null;
        if (!down && local.y < -0.5) return null;
        if (!right && local.x > 0.5) return null;
        if (!left && local.x < -0.5) return null;

        return new MoveCommand(this, movement.normalized);
    }

}