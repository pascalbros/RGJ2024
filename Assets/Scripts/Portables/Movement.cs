using UnityEngine;

public class Movement: Portable {
    public Vector2 direction;

    public override Command CanMove(Vector2 movement)
    {
        var filtered = (direction * movement);
        if (filtered.sqrMagnitude < 0.01)
            return null;

        return new MoveCommand(filtered.normalized);
    }

}