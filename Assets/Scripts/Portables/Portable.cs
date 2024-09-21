
using UnityEngine;

public class Portable: MonoBehaviour {
    public virtual Command CanMove(Vector2Int direction) {
        return null;
    }

    public virtual Command GetAction() {
        return null;
    }

    public virtual void Reflect(Vector2Int direction) {}
    
}