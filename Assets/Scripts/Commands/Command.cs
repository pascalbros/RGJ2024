using UnityEngine;

public abstract class Command 
{
    public Portable source;

    public int SourceUsages { get { return source == null ? -1 : source.usages; } }

    public void Do(PlayerController controller) {
        DoInner(controller);
        if (source == null) return;
        source.Use();
        if (source.IsExausted)
            controller.HandleConsumed(source);
    }

    public void Undo(PlayerController controller){
        UndoInner(controller);
        if (source == null) return;
        source.UndoUsage();
    }

    protected abstract void DoInner(PlayerController controller);

    protected abstract void UndoInner(PlayerController controller);
}
