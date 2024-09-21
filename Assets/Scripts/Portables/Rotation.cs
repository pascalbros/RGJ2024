
public class Rotation : Portable
{
    public bool clockwise;

    public override Command GetAction() {
        return new RotateCommand(this, clockwise);
    }
}
