using UnityEngine;

public interface Command 
{
    
    public void Do(PlayerController controller);

    public void Undo(PlayerController controller);
}
