using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Warp destination;

    void Awake() {
        var warps = GameObject.FindGameObjectsWithTag("Warp");
        Debug.Log(warps);
        Debug.Log(warps[0]);
        Debug.Log(warps[1]);
        GameObject destGo;
        if (warps[0] != this.gameObject) 
            destGo = warps[0];
        else
            destGo = warps[1];
        destination = destGo.GetComponent<Warp>();
    }

    public Command GetCommand(PlayerController controller) {
        return new WarpCommand(this, destination, controller);
    }
}
