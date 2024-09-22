using UnityEngine;
using System;

public class WarpCommand: Command {
    private Vector3 wsource;
    private Vector3 destination;
    private Command lastCommand;

    public WarpCommand(Warp source, Warp destination, PlayerController controller) {
        lastCommand = controller.LastCommand;
        this.wsource = source.transform.position;        
        this.destination = destination.transform.position;        
    }

    protected override void DoInner(PlayerController controller) {
        controller.ApplyWarp(destination);
    }

    protected override void UndoInner(PlayerController controller) {
        controller.UndoWarp(wsource);
        lastCommand.Undo(controller);
    }
}
