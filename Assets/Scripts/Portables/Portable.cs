
using UnityEngine;

public class Portable: MonoBehaviour {
    public int usages = -1;

    public void Use() {
        if (usages > 0) usages--;
    }

    public void UndoUsage() {
        if (usages >= 0) usages++;
    }

    public bool IsExausted { get { return usages == 0; } }

    public virtual MoveCommand CanMove(Vector2 direction) {
        return null;
    }

    public virtual Command GetAction() {
        return null;
    }

    public virtual void Reflect(Vector2Int direction) {}
    
}