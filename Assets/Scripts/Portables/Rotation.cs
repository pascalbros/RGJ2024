using UnityEngine;

public class Rotation : Portable
{
    public bool clockwise;

    public override Command GetAction() {
        var angles = transform.localEulerAngles;
        bool cw = clockwise;
        if (Mathf.Approximately((angles.x + angles.y) % 360, 180))
            cw = !cw;
        return new RotateCommand(this, cw);
    }
}
