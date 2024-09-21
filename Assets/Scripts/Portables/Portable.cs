
using UnityEngine;

public class Portable: MonoBehaviour {
    public int usages = -1;

    public GameObject bigIcon;
    public GameObject smallIcon;
    public GameObject smallVertIcon;

    void Awake() {
        bigIcon = transform.Find("Big").gameObject;
        smallIcon = transform.Find("Icon").gameObject;
        smallVertIcon = transform.Find("VertIcon").gameObject;
    }

    public void SetUsages(int value) {
        usages = value;
    }

    public void Use() {
        if (usages > 0) usages--;
    }

    public void UndoUsage() {
        if (usages >= 0) usages++;
    }

    public bool IsConsumable { get { return usages >= 0; } }
    public bool IsExausted { get { return usages == 0; } }

    public virtual MoveCommand CanMove(Vector2 direction) {
        return null;
    }

    public virtual Command GetAction() {
        return null;
    }

    public virtual bool IsKey { get { return false; } }

    public virtual void Reflect(Vector2Int direction) {}
    
}