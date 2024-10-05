using UnityEngine;

using strange.extensions.command.impl;

public class Player3Command : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
       
        Debug.Log("Player3:" + recData.player.Name);

        gameData.Players.Add(recData.player);
    }
}