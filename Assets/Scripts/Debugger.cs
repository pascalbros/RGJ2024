using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public PlayerController controller;

    [ContextMenuItem("Pickup", "Pickup")]
    public Portable top;
    [ContextMenuItem("Pickup", "Pickup")]
    public Portable bottom;

    [ContextMenu("Pickup")]
    public void Pickup() {
        controller.HandlePickup(top, true);
        controller.HandlePickup(bottom, false);
    }

    [ContextMenu("Reflect")]
    public void Reflect() {
        Debug.Log("reflect");
        controller.HandleReflection();
    }

}
