using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Command 
{
    
    public void Do(GameObject gameObject);

    public void Undo(GameObect gameObect);
}
