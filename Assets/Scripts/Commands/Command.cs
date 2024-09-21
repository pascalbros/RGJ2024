using UnityEngine;

public interface Command 
{
    
    public void Do(GameObject gameObject);

    public void Undo(GameObject gameObect);
}
