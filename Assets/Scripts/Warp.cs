using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Warp destination

    void Awake() {
        var warps = FindGameObjectsWithTag<Warp>();
        if (warps[0] != this) 
            destination = warps[0];
        else
            destination = warps[1];
    }

    public Command GetCommand(PlayerController controller) {
        return new WarpCommand(destination.transform.position, controller);
    }
}
