using UnityEngine;

public abstract class Command 
{
    public Portable source;

    public int SourceUsages { get { return source == null ? -1 : source.usages; } }

    public void Do(PlayerController controller) {
        if (source != null) {
            source.Use();
            if (source.IsExausted)
                controller.HandleConsumed(source);
        }
        DoInner(controller);
    }

    public void Undo(PlayerController controller){
        UndoInner(controller);
        if (source != null)
            source.UndoUsage();
    }

    protected abstract void DoInner(PlayerController controller);

    protected abstract void UndoInner(PlayerController controller);
}
