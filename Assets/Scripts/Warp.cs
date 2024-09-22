using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Warp destination;

    void Awake() {
        Setup();
    }

    private void Setup() {
        if (destination != null) return;
        var warps = GameObject.FindGameObjectsWithTag("Warp");
        GameObject destGo = null;
        if (warps[0] != this.gameObject) 
            destGo = warps[0];
        else if (warps.Length > 1)
            destGo = warps[1];
        destination = destGo?.GetComponent<Warp>();
    }

    public Command GetCommand(PlayerController controller) {
        Setup();
        return new WarpCommand(this, destination, controller);
    }
}
